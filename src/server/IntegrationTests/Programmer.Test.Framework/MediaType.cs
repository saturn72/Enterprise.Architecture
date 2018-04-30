using System.Net.Http.Headers;

namespace Programmer.Test.Framework
{
    public class MediaType
    {
        #region Members
        public static readonly MediaType ApplicationJson = new MediaType("application/json");
        public static readonly MediaType ApplicationXJavascript = new MediaType("application/x-javascript");
        public static readonly MediaType AudioL24 = new MediaType("audio/L24");
        public static readonly MediaType ApplicationXShockwaveFlash = new MediaType("application/x-shockwave-flash");
        public static readonly MediaType ApplicationOctetStream = new MediaType("application/octet-stream");
        public static readonly MediaType ApplicationAtomXml = new MediaType("application/atom+xml");
        public static readonly MediaType TextPlain = new MediaType("text/plain");
        public static readonly MediaType TextCss = new MediaType("text/css");
        public static readonly MediaType TextCmd = new MediaType("text/cmd");
        public static readonly MediaType TextCsv = new MediaType("text/csv");
        public static readonly MediaType ApplicationXDeb = new MediaType("application/x-deb");
        public static readonly MediaType MultipartEncrypted = new MediaType("multipart/encrypted");
        public static readonly MediaType MultipartFormData = new MediaType("multipart/form-data");
        public static readonly MediaType MultipartSigned = new MediaType("multipart/signed");
        public static readonly MediaType MessageHttp = new MediaType("message/http");

        #endregion

        #region Fields
        private readonly string _name;
        private readonly MediaTypeHeaderValue _mediaTypeHeaderValue;

        #endregion

        #region Properties
        public string Name => _name;

        public MediaTypeHeaderValue MediaTypeHeaderValue => _mediaTypeHeaderValue;

        #endregion
        #region ctor
        public MediaType(string name)
        {
            _name = name;
            _mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(_name);
        }
        #endregion
    }
}