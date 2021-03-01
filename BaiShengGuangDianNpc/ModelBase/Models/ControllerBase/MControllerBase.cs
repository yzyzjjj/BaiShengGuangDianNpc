namespace ModelBase.Models.ControllerBase
{
    public interface IMControllerBase
    {
    }

    public class MControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase, IMControllerBase
    {
        public readonly string UserId;
        public MControllerBase(string userId) : base()
        {
            UserId = userId;
        }
    }
}
