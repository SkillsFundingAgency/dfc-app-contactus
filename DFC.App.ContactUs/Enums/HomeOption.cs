using System.ComponentModel;

namespace DFC.App.ContactUs.Enums
{
    public enum HomeOption
    {
        [Description("None")]
        None,
        [Description("Speak to an adviser using webchat")]
        Webchat,
        [Description("Send us an online message. We'll email you back")]
        SendAMessage,
        [Description("Ask us to call you back")]
        Callback,
        [Description("Send us a letter")]
        Sendletter,
    }
}