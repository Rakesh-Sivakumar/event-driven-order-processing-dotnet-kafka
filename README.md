# Event-Driven Order Processing System (.NET + Kafka + Redis)

## 🚀 Overview

This project is an event-driven backend system built using .NET 8, Kafka, Redis, and SQL Server.
It simulates a real-world order processing workflow where services communicate asynchronously using Kafka.

The system is designed using Clean Architecture principles to ensure separation of concerns, scalability, and maintainability.

---

## 🧱 Architecture

### Components:

* **Order.API** → Accepts order requests
* **Kafka** → Event streaming platform
* **Payment.Worker** → Processes payment events
* **Inventory.Worker** → Updates stock and manages cache
* **Redis** → Caching layer for inventory data
* **SQL Server** → Persistent data storage

---

## 🔄 Workflow

1. User places an order via Order API
2. Order is stored in SQL Server
3. `OrderCreatedEvent` is published to Kafka
4. Payment Worker consumes event and processes payment
5. `PaymentCompletedEvent` is published
6. Inventory Worker consumes event and updates stock
7. Redis cache is invalidated and refreshed

---

## 🧰 Tech Stack

* .NET 8 (Web API + Worker Services)
* Apache Kafka
* Redis
* SQL Server
* Docker & Docker Compose
* Serilog (Logging)

---

## 🧠 Key Features

* Event-driven architecture using Kafka
* Asynchronous processing with workers
* Redis caching with cache invalidation strategy
* Idempotent event handling
* Clean Architecture (Domain, Application, Infrastructure)
* Dependency Injection
* Structured logging

---

## 📂 Project Structure

```
src/
  Order.API
  Payment.Worker
  Inventory.Worker

core/
  Order.Domain
  Order.Application

infrastructure/
  Order.Infrastructure

shared/
  Shared.Contracts
```

---

## ▶️ How to Run

### 1. Start Infrastructure

```bash
docker-compose up -d
```

### 2. Run Services

Run the following projects:

* Order.API
* Payment.Worker
* Inventory.Worker

---

### 3. Test API

POST request:

```json
{
  "productName": "phone",
  "quantity": 2
}
```

---

## 📌 Logging

* Console logging for development
* File logging using Serilog
* Structured logs with contextual data (OrderId, EventId)

---

## ⚠️ Limitations / Improvements

* No authentication/authorization
* No retry or dead-letter queue
* No centralized logging (ELK/Grafana)
* No API Gateway

---

## 💬 Interview Talking Points

* Designed an event-driven system using Kafka for decoupled communication
* Implemented Redis caching with proper invalidation strategy
* Applied Clean Architecture to separate business logic from infrastructure
* Built asynchronous workers for background processing
* Used structured logging for traceability

---

## 🧠 Key Learnings

* Handling eventual consistency in distributed systems
* Managing cache consistency with Redis
* Designing loosely coupled services using events
* Structuring .NET applications using Clean Architecture

---
