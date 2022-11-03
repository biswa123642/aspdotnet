using System;
using System.Linq;
using ComponentArt.Web.UI;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using Sitecore.Controls;

namespace CGP.Foundation.SitecoreExtensions.sitecore_modules.Shell.CustomFields.ColorPicker.Dialogs
{
    public class ColorPickerDialog : DialogPage
    {
        // Fields
        protected ComponentArt.Web.UI.ColorPicker ColorPicker1;

        // Methods
        protected override void OK_Click()
        {
            SheerResponse.SetDialogValue(ColorPicker1.SelectedColor.Hex);
            base.OK_Click();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!AjaxScriptManager.IsEvent)
            {
                var value = WebUtil.GetQueryString("value");

                if (!String.IsNullOrEmpty(value))
                {
                    if(!value.ToString().Contains('#'))
                    {
                        value = "#FFFFFF";
                    }
                    ColorPicker1.SelectedColor = new ColorPickerColor { Hex = value };
                }
                ColorPicker1.Colors.Add(new ColorPickerColor { Name = "Red" });
                ColorPicker1.Colors.Add(new ColorPickerColor { Name = "Orange" });
                ColorPicker1.Colors.Add(new ColorPickerColor { Name = "Yellow" });
                ColorPicker1.Colors.Add(new ColorPickerColor { Name = "Green" });
                ColorPicker1.Colors.Add(new ColorPickerColor { Name = "Blue" });
                ColorPicker1.Colors.Add(new ColorPickerColor { Name = "Purple" });
                ColorPicker1.Colors.Add(new ColorPickerColor { Name = "Black" });
                ColorPicker1.Colors.Add(new ColorPickerColor { Name = "White" });
                ColorPicker1.ClientEvents.ColorChanged = new ClientEvent("colorChanged");
                ColorPicker1.ClientEvents.Load = new ClientEvent("load");
            }
        }
    }
}
