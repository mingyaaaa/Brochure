namespace Brochure.ORM.Visitors
{
    public interface IFuncVisit
    {
        string FuncName { get; }

        string GetExcuteSql (object call, object member);
    }
}