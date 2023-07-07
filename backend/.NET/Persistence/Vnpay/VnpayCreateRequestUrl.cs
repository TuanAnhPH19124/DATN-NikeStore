using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Vnpay
{
  public static class VnpayCreateRequestUrl
  {
    public static string Init()
    {
      var uriBuilder = new UriBuilder();
      uriBuilder.Scheme = "https";
      uriBuilder.Host = "sandbox.vnpayment.vn";
      uriBuilder.Path = "/paymentv2/vpcpay.html";
      
      var url = uriBuilder.Uri;
      return url.ToString();
    }
  }
}