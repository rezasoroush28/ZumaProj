public interface IUserSessionService
{
    void SetUserState(long chatId, UserState state);
    UserState GetUserState(long chatId);
    void SetExpectedInput(long chatId, ExpectedInputType inputType);
    ExpectedInputType GetExpectedInput(long chatId);
    void ClearExpectedInput(long chatId);
}

public enum UserState
{
    Idle,
    WaitingForAnswer
}

public enum ExpectedInputType
{
    None,              // یعنی الان منتظر هیچ پاسخی نیستیم (همون حالت Idle)
    TodoTitle,
    TodoDescription,
    // در آینده می‌تونه بشه: AiPrompt, Feedback, PhoneNumber و غیره...
}

