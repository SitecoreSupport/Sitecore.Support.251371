namespace Sitecore.Support.Analytics.Pipelines.EndAnalytics
{
  using Sitecore.Analytics;
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;
  using System.Collections.Generic;
  using System.Web;
  using System.Xml;
  using Sitecore.Xml;
  using System;

  public class ExcludeUrls
  {
    private readonly List<string> startUrls = new List<string>();

    public virtual void AddStartUrl(XmlNode configNode)
    {
      Assert.ArgumentNotNull(configNode, "configNode");
      string path = XmlUtil.GetAttribute("path", configNode);
      startUrls.Add(path);
    }

    public virtual void Process(PipelineArgs args)
    {
      if (HttpContext.Current != null && HttpContext.Current.Request != null &&
          HttpContext.Current.Request.FilePath != null)
      {
        foreach (var startUrl in startUrls)
        {
          if (HttpContext.Current.Request.FilePath.StartsWith(startUrl, StringComparison.OrdinalIgnoreCase))
          {
            if (Tracker.Current != null && Tracker.Current.CurrentPage != null)
            {
              Tracker.Current.Interaction.CurrentPage.Cancel();
            }
          }
        }
      }
    }
  }
}