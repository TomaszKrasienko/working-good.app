// using wg.modules.tickets.domain.Exceptions;
// using wg.modules.tickets.domain.Repositories;
// using wg.shared.abstractions.CQRS.Commands;
// using wg.shared.abstractions.Time;
//
// namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketState;
//
// internal sealed class ChangeTicketStateCommandHandler(
//     ITicketRepository ticketRepository, IClock clock) : ICommandHandler<ChangeTicketStateCommand>
// {
//     public async Task HandleAsync(ChangeTicketStateCommand command, CancellationToken cancellationToken)
//     {
//         var ticket = await ticketRepository.GetByIdAsync(command.Id);
//         if (ticket is null)
//         {
//             throw new TicketNotFoundException(command.Id);
//         }
//         
//         ticket.ChangeState(command.State, clock.Now());
//         await ticketRepository.UpdateAsync(ticket);
//     }
// }