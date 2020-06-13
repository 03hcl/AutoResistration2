namespace AcanthusPortalLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// アカンサスポータルでの処理を取り扱うための基本クラスです。
    /// </summary>
    public class AcanthusBase : IDisposable
    {
        #region プロパティ

        /// <summary>
        /// HttpClient を取得または設定します。
        /// </summary>
        public AcanthusHttpClient HttpClient { get; set; }

        #endregion

        /// <summary>
        /// ログイン操作を非同期で実行して、セッションに使われる <see cref="Cookie"/> を取得します。
        /// </summary>
        /// <param name="id"> 金沢大学ID。 </param>
        /// <param name="password"> パスワード。 </param>
        /// <param name="defaultCookie"> 既定の <see cref="Cookie"/> 。 </param>
        /// <param name="baseAddress"> 要求を送信するときの URI のベースアドレス。 </param>
        /// <param name="startFunc"> 最初に実行される HTTP 要求。 </param>
        /// <param name="goalUri"> ログイン完了後に到達したいページの URI 。 </param>
        /// <returns> ログイン操作が成功したかどうか。 </returns>
        protected async Task<Cookie> LoginAndGetCookies(
            string id,
            string password,
            Cookie defaultCookie,
            string baseAddress,
            Func<Task<HttpResponseMessage>> startFunc,
            string goalUri)
        {
            var maxRetry = 3;
            var maxRedirect = 8;

            for (var retry = 0; retry < maxRetry; retry++)
            {
                var cc = new CookieContainer();
                if (defaultCookie != null) { cc.Add(new Uri(baseAddress), defaultCookie); }
                this.HttpClient = new AcanthusHttpClient(baseAddress, new HttpClientHandler() { CookieContainer = cc });

                try
                {
                    var response = await startFunc();

                    Match formMatch;
                    int redirectCount = 0;

                    var html = string.Empty;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var reader = (new StreamReader(stream, Encoding.GetEncoding("shift_jis"), true)) as TextReader)
                    {
                        html = await reader.ReadToEndAsync();
                    }

                    while ((redirectCount < maxRedirect) && (formMatch = Regex.Match(
                        await response.Content.ReadAsStringAsync(),
                        "<form[^<]*action=\"(?<action>[^\"]*)\"[^<]*>(?<data>.*?)</form[^<]*>",
                        RegexOptions.Singleline | RegexOptions.IgnoreCase)).Success)
                    {
                        if (response.RequestMessage?.RequestUri?.AbsoluteUri == goalUri)
                        {
                            ////this.KanazawaUniversityID = id;

                            foreach (Cookie cookie in cc.GetCookies(response.RequestMessage.RequestUri))
                            {
                                if (cookie.Name.Contains("shibsession")) { return cookie; }
                            }

                            return null;
                            ////return cc.GetCookies(response.RequestMessage.RequestUri);
                        }

                        var formData = new Dictionary<string, string>();

                        for (var tagMatch = Regex.Match(
                                formMatch.Groups["data"].Value,
                                "<[^<]*?name=\"(?<name>[^\"]*)\"[^<]*?(?:value=\"(?<value>[^\"]*)\"[^<]*?)?>",
                                RegexOptions.Singleline | RegexOptions.IgnoreCase);
                            tagMatch.Success;
                            tagMatch = tagMatch.NextMatch())
                        {
                            var tagName = tagMatch.Groups["name"].Value;
                            string tagValue;

                            if (tagName == "j_username") { tagValue = id; }
                            else if (tagName == "j_password") { tagValue = password; }
                            else { tagValue = tagMatch.Groups["value"]?.Value; }

                            formData.Add(WebUtility.HtmlDecode(tagName), WebUtility.HtmlDecode(tagValue));
                        }

                        var content = new FormUrlEncodedContent(formData);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                        this.HttpClient.DefaultRequestHeaders.Referrer = response.RequestMessage.RequestUri;
                        response = await this.HttpClient.PostAsync(
                            new Uri(response.RequestMessage.RequestUri, WebUtility.HtmlDecode(formMatch.Groups["action"].Value)),
                            content);

                        redirectCount++;
                    }
                }
                catch
                {
                }
            }

            this.HttpClient.Dispose();
            ////this.KanazawaUniversityID = string.Empty;
            return null;
        }

        /// <summary>
        /// ログイン操作を非同期で実行します。
        /// </summary>
        /// <param name="id"> 金沢大学ID。 </param>
        /// <param name="password"> パスワード。 </param>
        /// <param name="baseAddress"> 要求を送信するときの URI のベースアドレス。 </param>
        /// <param name="startFunc"> 最初に実行される HTTP 要求。 </param>
        /// <param name="goalUri"> ログイン完了後に到達したいページの URI 。 </param>
        /// <returns> ログイン操作が成功したかどうか。 </returns>
        protected async Task<bool> Login(string id, string password, string baseAddress, Func<Task<HttpResponseMessage>> startFunc, string goalUri)
        => await this.LoginAndGetCookies(id, password, null, baseAddress, startFunc, goalUri) != null;

        /// <summary>
        /// ログイン操作を非同期で実行します。
        /// </summary>
        /// <param name="defaultCookie"> 既定の <see cref="Cookie"/> 。 </param>
        /// <param name="baseAddress"> 要求を送信するときの URI のベースアドレス。 </param>
        /// <param name="startFunc"> 最初に実行される HTTP 要求。 </param>
        /// <param name="goalUri"> ログイン完了後に到達したいページの URI 。 </param>
        /// <returns> ログイン操作が成功したかどうか。 </returns>
        protected async Task<bool> Login(Cookie defaultCookie, string baseAddress, Func<Task<HttpResponseMessage>> startFunc, string goalUri)
        => await this.LoginAndGetCookies(null, null, defaultCookie, baseAddress, startFunc, goalUri) != null;

        /// <summary>
        /// ログアウト操作を非同期で実行します。
        /// ただし、成功か失敗かにかかわらず HttpClient とその接続は破棄されます。
        /// </summary>
        /// <param name="uri"> ログアウト時にアクセスする URI 。 </param>
        /// <returns> ログアウト操作が成功したかどうか。 </returns>
        protected async Task<bool> Logout(string uri)
        {
            var result = false;
            ////if (string.IsNullOrEmpty(this.KanazawaUniversityID)) { return true; }

            try
            {
                await this.HttpClient.GetAsync(uri);
                result = true;
            }
            finally
            {
                this.HttpClient.Dispose();
                ////this.KanazawaUniversityID = string.Empty;
            }

            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        /// <summary>
        /// <see cref="AcanthusBase"/> が使用しているアンマネージ リソースを解放し、マネージ リソースを破棄します。
        /// </summary>
        /// <param name="disposing"> 重複する呼び出しを検出するための引数。 </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.HttpClient.Dispose();
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~AcanthusBase() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        /// <summary>
        /// <see cref="AcanthusBase"/> が使用しているアンマネージ リソースを解放し、マネージ リソースを破棄します。
        /// </summary>
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
