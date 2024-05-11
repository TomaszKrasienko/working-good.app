// using NSubstitute;
// using Shouldly;
// using wg.modules.tickets.application.Clients.Companies;
// using wg.modules.tickets.application.Clients.Companies.DTO;
// using wg.modules.tickets.application.Clients.Owner;
// using wg.modules.tickets.application.Clients.Owner.DTO;
// using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignProject;
// using wg.modules.tickets.application.Exceptions;
// using wg.modules.tickets.domain.Exceptions;
// using wg.modules.tickets.domain.Repositories;
// using wg.modules.tickets.domain.ValueObjects.Ticket;
// using wg.shared.abstractions.CQRS.Commands;
// using wg.tests.shared.Factories.Tickets;
// using Xunit;
//
// namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;
//
// public sealed class AssignProjectCommandHandlerTests
// {
//     private Task Act(AssignProjectCommand command) => _handler.HandleAsync(command, default);
//
//     [Fact]
//     public async Task
//         HandleAsync_GivenExistingTicketWithAssignedUserAndAssignedEmployee_ShouldAssignProjectToTicketAndUpdate()
//     {
//         //arrange
//         var userId = Guid.NewGuid();
//         var ticket = TicketsFactory.GetOnlyRequired(State.Open());
//         ticket.ChangeAssignedUser(userId, DateTime.Now);
//         ticket.ChangeAssignedEmployee(Guid.NewGuid());
//
//         var membershipDto = new IsGroupMembershipExists()
//         {
//             Value = true
//         };
//
//         var isProjectForCompanyDto = new IsProjectForCompanyDto()
//         {
//             Value = true
//         };
//
//         var command = new AssignProjectCommand(Guid.NewGuid(), Guid.NewGuid());
//
//         _ticketRepository
//             .GetByIdAsync(ticket.Id)
//             .Returns(ticket);
//
//         _ownerApiClient
//             .IsMembershipExistsAsync(Arg.Is<GetMembershipDto>(arg
//                 => arg.GroupId == command.ProjectId
//                    && arg.UserId == userId))
//             .Returns(membershipDto);
//
//         _companiesApiClient
//             .IsProjectForCompanyAsync(Arg.Is<EmployeeWithProjectDto>(arg
//                 => arg.ProjectId == command.ProjectId
//                    && arg.EmployeeId == ticket.AssignedEmployee.Value))
//             .Returns(isProjectForCompanyDto);
//
//         //act
//         await Act(command);
//
//         //arrange
//         await _ticketRepository
//             .Received(1)
//             .UpdateAsync(ticket);
//
//         ticket.ProjectId.Value.ShouldBe(command.ProjectId);
//     }
//
//     [Fact]
//     public async Task
//         HandleAsync_GivenExistingTicketWithoutAssignedUserAndWithoutAssignedEmployee_ShouldAssignProjectToTicketAndUpdate()
//     {
//         //arrange
//         var userId = Guid.NewGuid();
//         var ticket = TicketsFactory.GetOnlyRequired(State.Open());
//         ticket.ChangeAssignedUser(userId, DateTime.Now);
//
//         var isProjectActiveDto = new IsProjectActiveDto()
//         {
//             Value = true
//         };
//
//         var command = new AssignProjectCommand(Guid.NewGuid(), Guid.NewGuid());
//
//         _ticketRepository
//             .GetByIdAsync(ticket.Id)
//             .Returns(ticket);
//
//         _companiesApiClient
//             .IsProjectActiveAsync(Arg.Is<ProjectIdDto>(arg
//                 => arg.Id == command.ProjectId))
//             .Returns(isProjectActiveDto);
//
//         //act
//         await Act(command);
//
//         //arrange
//         await _ticketRepository
//             .Received(1)
//             .UpdateAsync(ticket);
//
//         ticket.ProjectId.Value.ShouldBe(command.ProjectId);
//
//         await _ownerApiClient
//             .Received(0)
//             .IsMembershipExistsAsync(Arg.Any<GetMembershipDto>());
//
//         await _companiesApiClient
//             .Received(0)
//             .IsProjectForCompanyAsync(Arg.Any<EmployeeWithProjectDto>());
//     }
//
//     [Fact]
//     public async Task HandleAsync_GivenNotExistingTicket_ShouldThrowTicketNotFoundException()
//     {
//         //arrange
//         var command = new AssignProjectCommand(Guid.NewGuid(), Guid.NewGuid());
//
//         //act
//         var exception = await Record.ExceptionAsync(async () => await Act(command));
//
//         //assert
//         exception.ShouldBeOfType<TicketNotFoundException>();
//     }
//
//     [Fact]
//     public async Task HandleAsync_GivenNotActiveProject_ShouldThrowActiveProjectNotFoundException()
//     {
//         //arrange
//         var ticket = TicketsFactory.GetOnlyRequired(State.Open());
//         
//         _ticketRepository
//             .GetByIdAsync(ticket.Id)
//             .Returns(ticket);
//
//         var command = new AssignProjectCommand(ticket.Id, Guid.NewGuid());
//         
//         //act
//         var exception = await Record.ExceptionAsync(async () => await Act(command));
//         
//         //assert
//         exception.ShouldBeOfType<ActiveProjectNotFoundException>();
//     }
//
//
//
//     #region arrange
//     private readonly ITicketRepository _ticketRepository;
//     private readonly IOwnerApiClient _ownerApiClient;
//     private readonly ICompaniesApiClient _companiesApiClient;
//     private readonly ICommandHandler<AssignProjectCommand> _handler;
//     
//     public AssignProjectCommandHandlerTests()
//     {
//         _ticketRepository = Substitute.For<ITicketRepository>();
//         _ownerApiClient = Substitute.For<IOwnerApiClient>();
//         _companiesApiClient = Substitute.For<ICompaniesApiClient>();
//         _handler = new AssignProjectCommandHandler(_ticketRepository, _ownerApiClient);
//     }
//
//     #endregion
// }