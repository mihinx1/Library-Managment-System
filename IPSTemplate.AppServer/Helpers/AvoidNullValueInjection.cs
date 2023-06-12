using System.Reflection;
using Omu.ValueInjecter.Injections;

namespace IPSTemplate.AppServer.Helpers;

public class AvoidNullValueInjection : LoopInjection
{
    protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
    {
        var value = sp.GetValue(source);
        if (value is not null)
        {
            tp.SetValue(target, value);
        }
    }
}
