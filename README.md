**For 100commitow (https://100commitow.pl) contest by devmentors.io**

<div>
    <img src="assets/logo.png" width="500">
</div>

**What is WorkingGood?**



### Purpose:
This ticketing application is designed for small organizations to efficiently manage support tickets, project development, and track user time spent on issue resolution.

### Key Features:
- **Support Ticket Management:** Centralized platform for submitting support requests, reporting issues, and seeking assistance.
- **Project Development:** Tool for creating, assigning, and tracking tasks related to ongoing projects.
- **Time Tracking:** Ability to monitor time spent by users on resolving tickets or working on projects.
- **Customization and Scalability:** Flexible customization to adapt to specific organizational needs and scalable to accommodate growth.
- **Reporting and Analytics:** Built-in capabilities for generating insights into ticket resolution times, project progress, and team performance.

---

### Architecture

The system is built as a modular monolith, allowing for individual modules to be separated. It follows Domain-Driven Design (DDD), Event-Driven Architecture (EDA), and Test-Driven Development (TDD) principles.
- **Modular Monolith:**
Structured as a monolithic application with a modular design approach.
Modules can be developed, deployed, and scaled independently.
- **Domain-Driven Design (DDD):**
Models and organizes core business logic.
Aligns software design closely with the business domain.
- **Event-Driven Architecture (EDA):**
Components communicate through asynchronous events.
Enables scalability, resilience, and flexibility.
- **Test-Driven Development (TDD):**
Rigorous approach followed throughout development.
Includes both unit tests and integration tests for each class or module.
Overall, the system's architecture aims to deliver a robust, scalable, and maintainable software solution.
