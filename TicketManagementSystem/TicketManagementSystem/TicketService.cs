using System;

namespace TicketManagementSystem
{
    public class TicketService
    {
        private readonly ITicketRepository ticketRepository;
        private readonly UserRepository userRepository;
        public TicketService(ITicketRepository ticketRepository, UserRepository userRepository)
        {
            this.ticketRepository = ticketRepository;
            this.userRepository = userRepository;
        }

        public TicketService(ITicketRepository ticketRepository) 
            : this(ticketRepository, new UserRepository()) 
        {
        }

        public int CreateTicket(string title, Priority priority, string assignedTo, string description, DateTime created, bool isPayingCustomer)
        {
            ValidateTicketInputs(title, description);
            var assignedUser = GetUser(assignedTo);
            priority = AdjustPriority(title, priority, created);

            double price = GetTicketPrice(priority, isPayingCustomer);
            User accountManager = isPayingCustomer ? userRepository.GetAccountManager() : null;

            var ticket = new Ticket {
                Title = title,
                Priority = priority,
                AssignedUser = assignedUser,
                Description = description,
                Created = created,
                PriceDollars = price,
                AccountManager = accountManager
            };

            return ticketRepository.CreateTicket(ticket);
        }

        private static void ValidateTicketInputs(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            {
                throw new InvalidTicketException("Title or description were null");
            }
        }

        private static User GetUser(string username)
        {
            var userRepository = new UserRepository();
            var user = userRepository.GetUser(username) ?? throw new UnknownUserException($"User {username} not found");
            return user;
        }

        public void AssignTicket(int ticketId, string username) {
            var ticket = ticketRepository.GetTicket(ticketId) ?? throw new InvalidTicketException($"No ticket found for id {ticketId}");
            ticket.AssignedUser = GetUser(username);

            ticketRepository.UpdateTicket(ticket);
        }

        private static Priority AdjustPriority(string title, Priority priority, DateTime created)
        {
            bool needsPriorityUpgrade = DateTime.UtcNow - created > TimeSpan.FromHours(1) 
                                        || title.Contains("Crash") 
                                        || title.Contains("Important") 
                                        || title.Contains("Failure");

            if (needsPriorityUpgrade)
            {
                priority = UpgradePriority(priority);
            }

            return priority;
        }

        private static Priority UpgradePriority(Priority priority)
        {
            return priority == Priority.Low ? Priority.Medium : Priority.High;
        }

        private static double GetTicketPrice(Priority priority, bool isPayingCustomer)
        {
            if (!isPayingCustomer) return 0;
            return priority == Priority.High ? 100 : 50;
        }
    }
}
