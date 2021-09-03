namespace BoringSoftware.Finances.Core.Utils
{
    public interface IUrlHelper
    {
        string Action(string action, object values = null);
    }
}
