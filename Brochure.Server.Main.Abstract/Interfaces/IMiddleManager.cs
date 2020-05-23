using System;
using Microsoft.AspNetCore.Http;

namespace Brochure.Server.Main.Abstract.Interfaces
{
    public interface IMiddleManager
    {
        void AddMiddle (Func<RequestDelegate, RequestDelegate> middle);
        void IntertMiddle (int index, Func<RequestDelegate, RequestDelegate> middle);

        void AddMiddle (Action action);
        void IntertMiddle (int index, Action action);

        int GetMiddleCount ();

        RequestDelegate UseMiddle ();
    }
}