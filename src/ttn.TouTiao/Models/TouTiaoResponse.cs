using System;
using System.Collections.Generic;
using System.Text;

namespace ttn.TouTiao.Models
{
    public class TouTiaoError
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public int error { get; set; }
        public string message { get; set; }
    }

    public class TouTiaoResponse<T> where T: class
    {
        /// <summary>
        ///     The response from the tt API is a success status.
        /// </summary>
        public bool IsSuccessStatus { get; set; }

        public string ResponseReasonPhrase { get; set; }

        /// <summary>
        ///     The <see cref="ResponseHeaders" /> for the API request.
        /// </summary>
        public ResponseHeaders Headers { get; set; }

        public TouTiaoError Error { get; set; }

        public T Response { get; set; }
    }
}
