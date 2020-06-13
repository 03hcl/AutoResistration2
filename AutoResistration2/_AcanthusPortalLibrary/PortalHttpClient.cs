namespace AcanthusPortalLibrary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// アカンサスポータルの HTTP 要求を処理するためのクラスを提供します。
    /// </summary>
    public class AcanthusHttpClient : HttpClient
    {
        #region コンストラクタ

        /// <summary>
        /// <see cref="AcanthusHttpClient"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="baseAddress"> 要求を送信するときの URI のベースアドレス。 </param>
        public AcanthusHttpClient(string baseAddress) : base()
        {
            this.InitializeHttpClient(baseAddress);
        }

        /// <summary>
        /// <see cref="AcanthusHttpClient"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="baseAddress"> 要求を送信するときの URI のベースアドレス。 </param>
        /// <param name="allowAutoRedirect"> クライアントが自動でリダイレクトすることを許可するかどうかを示す値。 </param>
        public AcanthusHttpClient(string baseAddress, bool allowAutoRedirect) : base(new HttpClientHandler() { AllowAutoRedirect = allowAutoRedirect })
        {
            this.InitializeHttpClient(baseAddress);
        }

        /// <summary>
        /// 指定したハンドラーを使用して <see cref="AcanthusHttpClient"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="baseAddress"> 要求を送信するときの URI のベースアドレス。 </param>
        /// <param name="handler"> 要求の送信に使用する HTTP ハンドラー スタック。 </param>
        public AcanthusHttpClient(string baseAddress, HttpClientHandler handler) : base(handler)
        {
            this.InitializeHttpClient(baseAddress);
        }

        #endregion

        #region メソッド(public)

        /// <summary>
        /// 指定された URI に GET 要求を非同期操作として送信し、リダイレクトが完了した時点での HTTP 応答メッセージを返します。
        /// HTTP 応答が失敗した場合は例外が返されます。
        /// </summary>
        /// <param name="requestUri"> 要求の送信先 URI。 </param>
        /// <returns> HTTP 応答メッセージ。 </returns>
        public async new Task<HttpResponseMessage> GetAsync(string requestUri)
            => await this.CompleteRedirectAsync(await base.GetAsync(requestUri));

        /// <summary>
        /// 指定された URI に GET 要求を非同期操作として送信し、リダイレクトが完了した時点での HTTP 応答メッセージを返します。
        /// HTTP 応答が失敗した場合は例外が返されます。
        /// </summary>
        /// <param name="requestUri"> 要求の送信先 URI。 </param>
        /// <returns> HTTP 応答メッセージ。 </returns>
        public async new Task<HttpResponseMessage> GetAsync(Uri requestUri)
            => await this.CompleteRedirectAsync(await base.GetAsync(requestUri));

        /// <summary>
        /// 指定された URI に POST 要求を非同期操作として送信し、リダイレクトが完了した時点での HTTP 応答メッセージを返します。
        /// HTTP 応答が失敗した場合は例外が返されます。
        /// </summary>
        /// <param name="requestUri"> 要求の送信先 URI。 </param>
        /// <param name="content"> サーバーに送信される HTTP 要求の内容。 </param>
        /// <returns> HTTP 応答メッセージ。 </returns>
        public async new Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
            => await this.CompleteRedirectAsync(await base.PostAsync(requestUri, content));

        /// <summary>
        /// 指定された URI に POST 要求を非同期操作として送信し、リダイレクトが完了した時点での HTTP 応答メッセージを返します。
        /// HTTP 応答が失敗した場合は例外が返されます。
        /// </summary>
        /// <param name="requestUri"> 要求の送信先 URI。 </param>
        /// <param name="content"> サーバーに送信される HTTP 要求の内容。 </param>
        /// <returns> HTTP 応答メッセージ。 </returns>
        public async new Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
            => await this.CompleteRedirectAsync(await base.PostAsync(requestUri, content));

        #endregion

        /// <summary>
        /// <see cref="HttpClient"/> を初期化します。
        /// </summary>
        /// <param name="baseAddress"> 要求を送信するときの URI のベースアドレス。 </param>
        protected void InitializeHttpClient(string baseAddress)
        {
            this.BaseAddress = new Uri(baseAddress);

            this.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.79 Safari/537.36");
            ////this.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:55.0) Gecko/20100101 Firefox/55.0");
            ////this.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.84 Safari/537.36");
            this.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            this.DefaultRequestHeaders.Add("Accept-Language", "ja,en-US;q=0.8,en;q=0.6");

            this.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            this.DefaultRequestHeaders.Add("Connection", "keep-alive");
            this.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        }

        /// <summary>
        /// 指定された HTTP 応答メッセージのリダイレクト要求を完了させ、その時点での HTTP 応答メッセージを返します。
        /// HTTP 応答が成功していない場合は例外が返されます。
        /// </summary>
        /// <param name="currentResponse"> 現在の HTTP 応答メッセージ。 </param>
        /// <returns> リダイレクトが完了した時点での HTTP 応答メッセージ。 </returns>
        protected async Task<HttpResponseMessage> CompleteRedirectAsync(HttpResponseMessage currentResponse)
        {
            var response = currentResponse;

            while (true)
            {
                while (response.StatusCode == HttpStatusCode.Found)
                {
                    this.DefaultRequestHeaders.Referrer = response.RequestMessage.RequestUri;
                    response = await base.GetAsync(response.Headers.Location);
                }

                var content = string.Empty;

                if (new List<string>(response.Content.Headers.ContentEncoding).Exists(s => Regex.Match(s, "gzip", RegexOptions.IgnoreCase).Success))
                {
                    using (var inStream = new MemoryStream(await response.Content.ReadAsByteArrayAsync()))
                    {
                        using (var gZipStream = new GZipStream(inStream, CompressionMode.Decompress))
                        using (var outStream = new MemoryStream())
                        {
                            await gZipStream.CopyToAsync(outStream);
                            content = Encoding.UTF8.GetString(outStream.ToArray());
                        }
                    }
                }
                else
                {
                    content = await response.Content.ReadAsStringAsync();
                }

                var match = Regex.Match(
                    content,
                    "<meta[^<]*http-equiv=\"refresh\"[^<]*content=\"\\d+(\\.\\d+)?;URL='?(?<url>.*?)'?\"[^<]*>", 
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);

                if (!match.Success) { break; }

                response = await base.GetAsync(new Uri(response.RequestMessage.RequestUri, WebUtility.HtmlDecode(match.Groups["url"]?.Value)));
            }

            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}
