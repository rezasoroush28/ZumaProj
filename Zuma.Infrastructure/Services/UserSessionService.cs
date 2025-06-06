using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuma.Infrastructure.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly Dictionary<long, UserState> _UserState = new();
        private readonly Dictionary<long, ExpectedInputType> _expectedInputType = new();
        public UserState GetUserState(long chatId)
        {
            return _UserState.TryGetValue(chatId, out var state)
                ? state : UserState.Idle;

        }

        public void SetUserState(long chatId, UserState state)
        {
            _UserState[chatId] = state;
        }

        public void SetExpectedInput(long chatId, ExpectedInputType inputType)
        {
            _expectedInputType[chatId] = inputType;
        }

        public ExpectedInputType GetExpectedInput(long chatId)
        {
            return _expectedInputType.TryGetValue(chatId, out var value) ? value : ExpectedInputType.None;
        }

        public void ClearExpectedInput(long chatId)
        {
            _expectedInputType.Remove(chatId);
        }


    }
}
