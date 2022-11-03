using Sitecore;
using Sitecore.Buckets.FieldTypes;
using Sitecore.Buckets.Util;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Web;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace CGP.Foundation.SitecoreExtensions.Commands
{
    public class FormattedMultiList : BucketList
    {
        private static Regex formatRegex = new Regex(@"\[format='(.+)'\]");
        private string _format;

        protected override void DoRender(HtmlTextWriter output)
        {
            if (!BucketConfigurationSettings.ItemBucketsEnabled())
            {
                output.Write(Translate.Text("The field cannot be displayed because the Item Buckets feature is disabled."));
            }
            else
            {
                this.GetStartLocation();
                this.RewriteSource();
                this.RenderStartLocationInput(output);
                this.BuildFilter();
                this.ServerProperties["ID"] = (object)this.ID;
                string str1 = string.Empty;
                if (this.ReadOnly)
                    str1 = " disabled='disabled'";
                output.Write("<input id='" + this.ID + "_Value' type='hidden' value='" + StringUtil.EscapeQuote(this.Value) + "' />");
                output.Write("<div class='scContentControlSearchListContainer'>");
                output.Write("<table" + this.GetControlAttributes() + ">");
                output.Write("<tr>");
                output.Write("<td class='scContentControlMultilistCaption' width='50%'>" + Translate.Text("All") + "</td>");
                output.Write("<td width='20'>" + Images.GetSpacer(20, 1) + "</td>");
                output.Write("<td class='scContentControlMultilistCaption' width='50%'>" + Translate.Text("Selected") + "</td>");
                output.Write("<td width='20'>" + Images.GetSpacer(20, 1) + "</td>");
                output.Write("</tr>");
                output.Write("<tr>");
                output.Write("<td valign='top' height='100%'>");
                output.Write("<div class='scMultilistNav'><input type='text' class='scIgnoreModified bucketSearch inactive' value='" + this.TypeHereToSearch + "' id='filterBox" + this.ClientID + "' " + (Sitecore.Context.ContentDatabase.GetItem(this.ItemID).Access.CanWrite() ? string.Empty : "disabled") + ">");
                output.Write("<a id='prev" + this.ClientID + "' class='hovertext'>" + Images.GetImage("Office/16x16/arrow_left.png", 16, 16, "absmiddle") + Translate.Text("Prev") + "</a>");
                output.Write("<a id='next" + this.ClientID + "' class='hovertext'> " + Translate.Text("Next") + Images.GetImage("Office/16x16/arrow_right.png", 16, 16, "absmiddle") + "</a>");
                output.Write("<a id='refresh" + this.ClientID + "' class='hovertext'> " + Translate.Text("Refresh") + Images.GetImage("Office/16x16/refresh.png", 16, 16, "absmiddle") + "</a>");
                output.Write("<a id='goto" + this.ClientID + "' class='hovertext'> " + Translate.Text("Go to item") + Images.GetImage("Office/16x16/magnifying_glass.png", 16, 16, "absmiddle") + "</a>");
                output.Write("<span><span><strong>" + Translate.Text("Page number") + ": </strong></span><span id='pageNumber" + this.ClientID + "'></span></span></div>");
                string str2 = !UIUtil.IsIE() || UIUtil.GetBrowserMajorVersion() != 9 ? "10" : "11";
                output.Write("<select id=\"" + this.ID + "_unselected\" class='scBucketListBox' multiple=\"multiple\" size=\"" + str2 + "\"" + str1 + " >");
                output.Write("</select>");
                output.Write("</td>");
                output.Write("<td valign='top'>");
                output.Write("<img class='' height='16' width='16' border='0' alt='' style='margin: 15px;' src='/sitecore/shell/themes/standard/Images/blank.png'/>");
                output.Write("<br />");
                this.RenderButton(output, "Office/16x16/navigate_right.png", string.Empty, "btnRight" + this.ClientID);
                output.Write("<br />");
                this.RenderButton(output, "Office/16x16/navigate_left.png", string.Empty, "btnLeft" + this.ClientID);
                output.Write("</td>");
                output.Write("<td valign='top' height='100%'>");
                output.Write("<select id='" + this.ID + "_selected' class='scBucketListSelectedBox' multiple='multiple' size='10'" + str1 + ">");
                Language language = Language.Parse(this.ItemLanguage);
                string str3 = this.Value;
                char[] separator = new char[1] { '|' };
                foreach (string path in str3.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                {
                    Item obj = Sitecore.Context.ContentDatabase.GetItem(path, language);
                    string str4 = obj != null ? this.GetDisplayNameForItem(obj) : path + " " + Translate.Text("[Item not found]");
                    output.Write("<option value='" + path + "'>" + str4 + "</option>");
                }
                output.Write("</select>");
                output.Write("</td>");
                output.Write("<td valign='top'>");
                output.Write("<img class='' height='16' width='16' border='0' alt='' style='margin: 15px 0;' src='/sitecore/shell/themes/standard/Images/blank.png'/>");
                output.Write("<br />");
                this.RenderButton(output, "Office/16x16/navigate_up.png", "javascript:scContent.multilistMoveUp('" + this.ID + "')", "btnUp" + this.ClientID);
                output.Write("<br />");
                this.RenderButton(output, "Office/16x16/navigate_down.png", "javascript:scContent.multilistMoveDown('" + this.ID + "')", "btnDown" + this.ClientID);
                output.Write("</td>");
                output.Write("</tr>");
                output.Write("<div style='border:1px solid #999999;font-size:8pt;display:none;padding:2px;margin:4px 0px 4px 0px;height:14px' id='" + this.ID + "_all_help'></div>");
                output.Write("<div style='border:1px solid #999999;font-size:8pt;display:none;padding:2px;margin:4px 0px 4px 0px;height:14px' id='" + this.ID + "_selected_help'></div>");
                output.Write("</table>");
                output.Write("</div>");
                //output.Write(string.Concat("<script type='text/javascript'>(function() {if (!document.getElementById('BucketListJs')) { var head = document.getElementsByTagName('head')[0]; head.appendChild(new Element('script', { type: 'text/javascript', src: '/sitecore/shell/Controls/BucketList/CustomBucketList.js', id: 'BucketListJs' }));head.appendChild(new Element('link', { rel: 'stylesheet', href: '/sitecore/shell/Controls/BucketList/BucketList.css' }));}var stopAt = Date.now() + 10000;var timeoutId = setInterval(function() {if (window.Sitecore && Sitecore.InitBucketList) {clearInterval(timeoutId);Sitecore.InitBucketList(", this.ScriptParameters, ");} else if (Date.now() > stopAt) {clearInterval(timeoutId);console.log('Unable to init Multilist with search control')}}, 100);}());</script>"));
                this.RenderScript(output);
            }
        }

        /// <summary>Renders the supporting JavaScript</summary>
        /// <param name="output">The writer.</param>
        protected override void RenderScript(HtmlTextWriter output)
        {
            string str = "<script type='text/javascript'>(function() {if (!document.getElementById('CustomBucketListJs')) { var head = document.getElementsByTagName('head')[0]; head.appendChild(new Element('script', { type: 'text/javascript', src: '/sitecore/shell/Controls/BucketList/CustomBucketList.js', id: 'CustomBucketListJs' }));head.appendChild(new Element('link', { rel: 'stylesheet', href: '/sitecore/shell/Controls/BucketList/BucketList.css' }));}var stopAt = Date.now() + 10000;var timeoutId = setInterval(function() {if (window.Sitecore && Sitecore.InitBucketList) {clearInterval(timeoutId);Sitecore.InitBucketList(" + this.ScriptParameters + ");} else if (Date.now() > stopAt) {clearInterval(timeoutId);console.log('Unable to init Multilist with search control')}}, 100);}());</script>";
            output.Write(str);
        }

        protected virtual void RewriteSource()
        {
            if (string.IsNullOrEmpty(this.Source))
            {
                return;
            }

            if (formatRegex.IsMatch(this.Source))
            {
                var match = formatRegex.Match(this.Source);
                _format = match.Groups[1].Value;
                this.Source = formatRegex.Replace(this.Source, string.Empty);
            }
        }

        private void GetStartLocation()
        {
            string source = this.Source;
            if (!string.IsNullOrWhiteSpace(source) && source.Contains("StartSearchLocation"))
            {
                try
                {
                    string startSearchLocation = source.Split('&')[0];
                    string removeStartSearchLocationKey = startSearchLocation.Replace("StartSearchLocation=", string.Empty);
                    string removeBracket1 = removeStartSearchLocationKey.Replace("[", string.Empty);
                    string query = removeBracket1.Replace("]", string.Empty);
                    var currentItem = Factory.GetDatabase("master").GetItem(this.ItemID);
                    SiteInfo currentSite = Factory.GetSiteInfoList().Where(s => s.RootPath != "" && currentItem.Paths.Path.ToLower().StartsWith(s.RootPath.ToLower()))
                                                                            .OrderByDescending(s => s.RootPath.Length)
                                                                            .FirstOrDefault();

                    Regex formatRegex = new Regex(@"\@templatename=(.+)");
                    var match = formatRegex.Match(query);

                    string templateName = (match != null && !string.IsNullOrWhiteSpace(match.Groups[1].Value)) ? match.Groups[1].Value.Replace("@templatename=", string.Empty) : string.Empty;
                    if (!string.IsNullOrWhiteSpace(templateName))
                    {
                        string textToReplace = match.Groups[0].Value;
                        query = query.Replace(textToReplace, string.Empty);
                        var lookupAreaPath = currentSite.RootPath + query;
                        var lookupFolder = currentItem.Database.GetItem(lookupAreaPath);
                        var attributeTargetFolder = lookupFolder.Axes.GetDescendants().Where(x => x.TemplateName.ToString().Equals(templateName)).FirstOrDefault()?.ID.ToString();
                        if (!string.IsNullOrWhiteSpace(attributeTargetFolder))
                        {
                            attributeTargetFolder = "StartSearchLocation=" + attributeTargetFolder;
                            this.Source = this.Source.Replace(startSearchLocation, attributeTargetFolder);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("ERROR occured in FormattedMultiList.GetStartLocation()", ex, this);
                }
            }
        }
        private string GetDisplayNameForItem(Item item)
        {
            return string.IsNullOrEmpty(_format) ? item.Name :
                _format.Replace("@name", item.Name)
                .Replace("@parentname", item.Parent.Name)
                .Replace("@grandparentname", item.Parent.Parent.Name);
        }
    }
}