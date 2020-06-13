namespace AcanthusPortalLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 学務情報サービス上の処理を取り扱うクラスです。
    /// </summary>
    public class StudentInformationService : AcanthusBase
    {
        /// <summary>
        /// <see cref="StudentInformationService"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public StudentInformationService() : base()
        {
        }

        /// <summary>
        /// 金沢大学IDとパスワードを用いた新しいセッションを作成して、学務情報サービスへのログイン操作を非同期で実行します。
        /// </summary>
        /// <param name="id"> 金沢大学ID。 </param>
        /// <param name="password"> パスワード。 </param>
        /// <returns> ログイン操作が成功したかどうか。 </returns>
        public async Task<bool> Login(string id, string password) => await this.LoginAndGetCookies(id, password) != null;

        /// <summary>
        /// Cookie に保存されたセッションを利用して、学務情報サービスへのログイン操作を非同期で実行します。
        /// </summary>
        /// <param name="cookie"> 既定の <see cref="Cookie"/> 。 </param>
        /// <returns> ログイン操作が成功したかどうか。 </returns>
        public async Task<bool> Login(Cookie cookie) => await this.LoginAndGetCookies(null, null, cookie) != null;

        /// <summary>
        /// 学務情報サービスへのログイン操作を非同期で実行して、セッションに使われる <see cref="Cookie"/> を取得します。
        /// </summary>
        /// <param name="id"> 金沢大学ID。 </param>
        /// <param name="password"> パスワード。 </param>
        /// <returns> セッションに使われる <see cref="Cookie"/> 。ログインに失敗した場合は <see cref="null"/> を返します。 </returns>
        public async Task<Cookie> LoginAndGetCookies(string id, string password)
            => await this.LoginAndGetCookies(id, password, null);

        /// <summary>
        /// 学務情報サービスからのログアウト操作を非同期で実行します。
        /// ただし、成功か失敗かにかかわらず HttpClient とその接続は破棄されます。
        /// </summary>
        /// <returns> ログアウト操作が成功したかどうか。 </returns>
        public async Task<bool> Logout() => await base.Logout("/portal/logout.aspx");

        /// <summary>
        /// 学務情報サービスへのログイン操作を非同期で実行して、セッションに使われる <see cref="Cookie"/> を取得します。
        /// </summary>
        /// <param name="id"> 金沢大学ID。 </param>
        /// <param name="password"> パスワード。 </param>
        /// <param name="cookie"> 既定の <see cref="Cookie"/> 。 </param>
        /// <returns> セッションに使われる <see cref="Cookie"/> 。ログインに失敗した場合は <see cref="null"/> を返します。 </returns>
        protected async Task<Cookie> LoginAndGetCookies(string id, string password, Cookie cookie) => await base.LoginAndGetCookies(
            id,
            password,
            cookie,
            "https://eduweb.sta.kanazawa-u.ac.jp/",
            async () => await this.HttpClient.GetAsync("https://eduweb.sta.kanazawa-u.ac.jp/"),
            "https://eduweb.sta.kanazawa-u.ac.jp/portal/StudentApp/Top.aspx");
    }
}
