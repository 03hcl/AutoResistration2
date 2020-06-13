namespace AcanthusPortalLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// アカンサスポータル上の処理を取り扱うクラスです。
    /// </summary>
    public class AcanthusPortal : AcanthusBase
    {
        /// <summary>
        /// <see cref="AcanthusPortal"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public AcanthusPortal() : base()
        {
        }

        /// <summary>
        /// 金沢大学IDとパスワードを用いた新しいセッションを作成して、アカンサスポータルへのログイン操作を非同期で実行します。
        /// </summary>
        /// <param name="id"> 金沢大学ID。 </param>
        /// <param name="password"> パスワード。 </param>
        /// <returns> ログイン操作が成功したかどうか。 </returns>
        public async Task<bool> Login(string id, string password) => await this.LoginAndGetCookies(id, password) != null;

        /// <summary>
        /// Cookie に保存されたセッションを利用して、アカンサスポータルへのログイン操作を非同期で実行します。
        /// </summary>
        /// <param name="cookies"> 既定の <see cref="Cookie"/> 。 </param>
        /// <returns> ログイン操作が成功したかどうか。 </returns>
        public async Task<bool> Login(Cookie cookies) => await this.LoginAndGetCookies(null, null, cookies) != null;

        /// <summary>
        /// アカンサスポータルへのログイン操作を非同期で実行して、セッションに使われる <see cref="Cookie"/> を取得します。
        /// </summary>
        /// <param name="id"> 金沢大学ID。 </param>
        /// <param name="password"> パスワード。 </param>
        /// <returns> セッションに使われる <see cref="Cookie"/> 。ログインに失敗した場合は <see cref="null"/> を返します。 </returns>
        public async Task<Cookie> LoginAndGetCookies(string id, string password)
            => await this.LoginAndGetCookies(id, password, null);

        /// <summary>
        /// アカンサスポータルからのログアウト操作を非同期で実行します。
        /// ただし、成功か失敗かにかかわらず HttpClient とその接続は破棄されます。
        /// </summary>
        /// <returns> ログアウト操作が成功したかどうか。 </returns>
        public async Task<bool> Logout() => await base.Logout("/logout.php");

        /// <summary>
        /// アカンサスポータルへのログイン操作を非同期で実行して、セッションに使われる <see cref="Cookie"/> を取得します。
        /// </summary>
        /// <param name="id"> 金沢大学ID。 </param>
        /// <param name="password"> パスワード。 </param>
        /// <param name="cookie"> 既定の <see cref="Cookie"/> 。 </param>
        /// <returns> セッションに使われる <see cref="Cookie"/> 。ログインに失敗した場合は <see cref="null"/> を返します。 </returns>
        protected async Task<Cookie> LoginAndGetCookies(string id, string password, Cookie cookie) => await base.LoginAndGetCookies(
            id,
            password,
            cookie,
            "https://acanthus.cis.kanazawa-u.ac.jp/",
            async () => await this.HttpClient.PostAsync(
                "/portallogin.php",
                new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "lang", "0" },
                })),
            "https://acanthus.cis.kanazawa-u.ac.jp/base/top.php");
    }
}
