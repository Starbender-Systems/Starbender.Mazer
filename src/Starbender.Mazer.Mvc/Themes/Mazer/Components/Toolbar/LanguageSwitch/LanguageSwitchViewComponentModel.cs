using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Starbender.Mazer.Mvc.Themes.Mazer.Components.Toolbar.LanguageSwitch;

public class LanguageSwitchViewComponentModel
{
    public LanguageInfo CurrentLanguage { get; set; }

    public List<LanguageInfo> OtherLanguages { get; set; }
}
