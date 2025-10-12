using System;
namespace C__NET5;

internal class Generic
{
    class Message
    {
        public string Text { get; set; }

        public Message(string text) => Text = text;
    }

    class EmailMessage : Message
    {
        public EmailMessage(string text) : base(text) { }
    }

    internal class Covariance
    {
        interface IMessenger<out T> // ограничение ковариантности; без out ошибка - на уровне дженериков компилятор не видит наследование - надо явно указывать
        {
            T WriteMessage(string text);
        }

        class EmailMessenger : IMessenger<EmailMessage>
        {
            public EmailMessage WriteMessage(string text)
            {
                return new EmailMessage($"Email: {text}");
            }
        }

        internal Covariance()
        {
            IMessenger<Message> outlook = new EmailMessenger();
            Message message = outlook.WriteMessage("Hello World");
            Console.WriteLine(message.Text);    // Email: Hello World

            IMessenger<EmailMessage> emailClient = new EmailMessenger();
            IMessenger<Message> messenger = emailClient; // более конкретный тип
            Message emailMessage = messenger.WriteMessage("Hi!");
            Console.WriteLine(emailMessage.Text);    // Email: Hi!
        }
    }

    internal class Contravariance
    {
        interface IMessenger<in T> // ограничение контрвариантности; без in ошибка
        {
            void SendMessage(T message);
        }

        class SimpleMessenger : IMessenger<Message>
        {
            public void SendMessage(Message message)
            {
                Console.WriteLine($"Отправляется сообщение: {message.Text}");
            }
        }

        internal Contravariance()
        {
            IMessenger<EmailMessage> outlook = new SimpleMessenger();
            outlook.SendMessage(new EmailMessage("Hi!"));

            IMessenger<Message> telegram = new SimpleMessenger();
            IMessenger<EmailMessage> emailClient = telegram; // более базовый тип
            emailClient.SendMessage(new EmailMessage("Hello"));
        }
    }

    internal class CovarianceContravariance 
    {
        interface IMessenger<in T, out K>
        {
            void SendMessage(T message);

            K WriteMessage(string text);
        }

        class SimpleMessenger : IMessenger<Message, EmailMessage>
        {
            public void SendMessage(Message message)
            {
                Console.WriteLine($"Отправляется сообщение: {message.Text}");
            }

            public EmailMessage WriteMessage(string text)
            {
                return new EmailMessage($"Email: {text}");
            }
        }

        internal CovarianceContravariance()
        {
            IMessenger<EmailMessage, Message> messenger = new SimpleMessenger();
            Message message = messenger.WriteMessage("Hello World"); // инстанцируем более конкретным типом (out)
            Console.WriteLine(message.Text);
            messenger.SendMessage(new EmailMessage("Test"));

            IMessenger<EmailMessage, EmailMessage> outlook = new SimpleMessenger();
            EmailMessage emailMessage = outlook.WriteMessage("Message from Outlook"); // инстанцируем тем же типом (инвариантность?)
            outlook.SendMessage(emailMessage);

            IMessenger<Message, Message> telegram = new SimpleMessenger();
            Message simpleMessage = telegram.WriteMessage("Message from Telegram"); // инстанцируем более конкретным типом (out)
            telegram.SendMessage(simpleMessage);

            IMessenger<Message, EmailMessage> foo = new SimpleMessenger();
        }
    }
}
