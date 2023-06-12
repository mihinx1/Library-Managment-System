using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary;

public class LocalizedStringLengthAttribute : StringLengthAttribute
{
    public LocalizedStringLengthAttribute(int maximumLength) : base(maximumLength)
    {
        ErrorMessageResourceType = typeof(Properties.Resources);
        ErrorMessageResourceName = nameof(Properties.Resources.Global_StringMax);
    }

    public LocalizedStringLengthAttribute(int maximumLength, int minimumLength) : base(maximumLength)
    {
        MinimumLength = minimumLength;
        ErrorMessageResourceType = typeof(Properties.Resources);
        ErrorMessageResourceName = nameof(Properties.Resources.Global_StringMinMax);
    }
}
