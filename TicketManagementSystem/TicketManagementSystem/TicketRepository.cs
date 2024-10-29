using System.Collections.Generic;
using System.Linq;

namespace TicketManagementSystem
{
    public class TicketRepository : ITicketRepository
    {
        private readonly List<Ticket> tickets = new();

        public int CreateTicket(Ticket ticket)
        {
            // Assume that the implementation of this method does not need to change.
            var currentHighestTicket = tickets.Any() ? tickets.Max(i => i.Id) : 0;
            var id = currentHighestTicket + 1;
            ticket.Id = id;

            tickets.Add(ticket);

            return id;
        }

        public void UpdateTicket(Ticket ticket)
        {
            // Assume that the implementation of this method does not need to change.
            var outdatedTicket = tickets.FirstOrDefault(t => t.Id == ticket.Id);

            if (outdatedTicket != null)
            {
                tickets.Remove(outdatedTicket);
                tickets.Add(ticket);
            }
        }

        public Ticket GetTicket(int id)
        {
            // Assume that the implementation of this method does not need to change.
            return tickets.FirstOrDefault(a => a.Id == id);
        }
    }
}
