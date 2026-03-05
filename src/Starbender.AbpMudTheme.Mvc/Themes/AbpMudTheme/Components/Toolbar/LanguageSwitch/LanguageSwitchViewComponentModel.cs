using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Starbender.AbpMudTheme.Mvc.Themes.AbpMudTheme.Components.Toolbar.LanguageSwitch;

public class LanguageSwitchViewComponentModel
{
    public LanguageInfo CurrentLanguage { get; set; }

    public List<LanguageInfo> OtherLanguages { get; set; }
}
