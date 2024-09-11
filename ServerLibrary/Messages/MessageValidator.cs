using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Messages
{
    internal class MessageValidator
    {
        private static string[] _correctTypes = { "register", "withdraw", "reject", "acknowledge", "message", "file", "config", "status" };
        private static string[] _correctModes = { "producer", "subscriber" };
        public static ValidationResult Validate(Message message)
        {
            if (string.IsNullOrEmpty(message.Type) || string.IsNullOrEmpty(message.Id) ||
                string.IsNullOrEmpty(message.Topic) || message.Timestamp == DateTime.MinValue)
            {
                return new ValidationResult(false, "Invalid or missing fields in the message.");
            }

            if (!_correctTypes.Contains(message.Type))
            {
                return new ValidationResult(false, "Message has incorrect type.");
            }

            if (message.Type == "register" || message.Type == "withdraw")
            {
                if (!_correctModes.Contains(message.Mode))
                {
                    return new ValidationResult(false, "Message has incorrect mode.");
                }
            }
            else if (message.Mode != String.Empty)
            {
                return new ValidationResult(false, "Message has incorrect mode.");
            }

            if (message.Type == "aknowledge" || message.Type == "reject")
            {
                if (!message.Payload.ContainsKey("timestampOfMessage") || !message.Payload.ContainsKey("topicOfMessage") ||
                    !message.Payload.ContainsKey("success") || !message.Payload.ContainsKey("message"))
                {
                    return new ValidationResult(false, "Message is missing required info in payload.");
                }
            }

            return new ValidationResult(true, "");
        }
    }
}
