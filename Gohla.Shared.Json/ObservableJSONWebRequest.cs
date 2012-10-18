using System;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using Newtonsoft.Json.Linq;

namespace Gohla.Shared.Json
{
    public class ObservableJSONWebRequest : ObservableWebRequest
    {
        public IObservable<JObject> Request(String url, Func<HttpWebRequest, HttpWebRequest> requestModifier)
        {
            IObservable<WebResponse> response = CreateResponse(CreateRequest(url, requestModifier));
            return response.SelectMany
            (
                r => ReponseToObservable<JObject, JObject>
                (
                    r,
                    s => JObject.Parse(new StreamReader(s).ReadToEnd()),
                    x => new JObject[] { x }.ToObservable()
                )
            );
        }
    }
}
