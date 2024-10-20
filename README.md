# AdCreative.ai Integration Case

## What I changed the original project structure

- I wanted to make the project more modular and testable. So, I have implemented the Hexagonal Architecture.
- I followed the Domain-Driven Design principles.
- I removed the tests cases in the Integration project and created a new project called `Integration.Tests` to test the integration project.

### Prerequisites

- .NET 7.0
- Docker
- In order to run integration tests, you need to have Docker installed on your machine and running the docker engine.

### Distributed System Scenario Weaknesses

- The system is not fault-tolerant. If the `Redis` service is down, the system will not work.
- The system is not scalable vertically. Redis service should be single.
- The requests might be taking long time because of the `Redis` service. The system should be designed to handle this situation.
