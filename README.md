# Builder Design Pattern

The **Builder Pattern** is a **Creational Design Pattern** used to create complex objects **step by step**.

> **Core idea:** Instead of creating an object with a large or confusing constructor, use a Builder to configure the object step by step and call `Build()` when you're ready.

---

# 📌 Simple Definition

> The Builder Pattern separates the construction of a complex object from its representation, allowing the same construction process to create different configurations of an object.

In simple words:

```text
Builder
   ↓
Configure
   ↓
Configure
   ↓
Configure
   ↓
Build()
   ↓
Object
```

---

# ❌ The Problem

Suppose we have an `Employee` class:

```csharp
public class Employee
{
    public string Name { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}
```

For a simple object, we can use object initialization:

```csharp
Employee employee = new Employee
{
    Name = "Salman",
    Department = "IT",
    Salary = 100000,
    Email = "salman@gmail.com",
    Address = "Karachi"
};
```

This is perfectly fine.

But imagine the class has **20–30 properties**.

A constructor could become difficult to read:

```csharp
Employee employee = new Employee(
    "Salman",
    "IT",
    100000,
    "salman@gmail.com",
    "Karachi",
    "123456",
    // ...
);
```

Now it becomes difficult to remember:

* Which argument is which?
* Which properties are required?
* Which properties are optional?
* What happens if there are many similar parameter types?

This is where Builder becomes useful.

---

# ✅ Solution: Builder Pattern

Instead of constructing everything at once:

```text
new Employee(...)
```

we build the object step by step:

```text
EmployeeBuilder
      ↓
SetName()
      ↓
SetDepartment()
      ↓
SetSalary()
      ↓
SetEmail()
      ↓
SetAddress()
      ↓
Build()
      ↓
Employee
```

---

# 🧩 Structure of Builder Pattern

A simple Builder implementation contains:

| Component   | Responsibility                           |
| ----------- | ---------------------------------------- |
| **Product** | The object being created                 |
| **Builder** | Provides methods to configure the object |
| **Client**  | Uses the builder to construct the object |

Example:

```text
       Client
          │
          ▼
  EmployeeBuilder
          │
     ┌────┼────┐
     ↓    ↓    ↓
   Name Salary Email
     │    │    │
     └────┼────┘
          ↓
        Build()
          ↓
       Employee
```

---

# 💻 Complete C# Example

## 1. Product — Employee

```csharp
public class Employee
{
    public string Name { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}
```

This is the object we want to construct.

It is called the **Product**.

---

# 2. Builder — EmployeeBuilder

```csharp
public class EmployeeBuilder
{
    private Employee _employee = new Employee();

    public EmployeeBuilder SetName(string name)
    {
        _employee.Name = name;
        return this;
    }

    public EmployeeBuilder SetDepartment(string department)
    {
        _employee.Department = department;
        return this;
    }

    public EmployeeBuilder SetSalary(decimal salary)
    {
        _employee.Salary = salary;
        return this;
    }

    public EmployeeBuilder SetEmail(string email)
    {
        _employee.Email = email;
        return this;
    }

    public EmployeeBuilder SetAddress(string address)
    {
        _employee.Address = address;
        return this;
    }

    public Employee Build()
    {
        return _employee;
    }
}
```

The `EmployeeBuilder` is responsible for gradually configuring the `Employee`.

---

# 🔗 The Important Part: `return this`

You will often see:

```csharp
public EmployeeBuilder SetName(string name)
{
    _employee.Name = name;

    return this;
}
```

The question is:

> What does `this` mean?

`this` refers to the **current `EmployeeBuilder` object**.

So:

```csharp
return this;
```

means:

> "Return the current builder so another builder method can be called."

This enables **method chaining**.

---

# 🔄 Method Chaining

Because every method returns the builder:

```csharp
return this;
```

we can write:

```csharp
Employee employee = new EmployeeBuilder()
    .SetName("Salman")
    .SetDepartment("IT")
    .SetSalary(100000)
    .SetEmail("salman@gmail.com")
    .SetAddress("Karachi")
    .Build();
```

Instead of:

```csharp
EmployeeBuilder builder = new EmployeeBuilder();

builder.SetName("Salman");
builder.SetDepartment("IT");
builder.SetSalary(100000);
builder.SetEmail("salman@gmail.com");
builder.SetAddress("Karachi");

Employee employee = builder.Build();
```

Both approaches work.

The first version is called **method chaining**.

---

# 🧠 What Actually Happens?

Consider:

```csharp
EmployeeBuilder builder = new EmployeeBuilder();
```

We have:

```text
EmployeeBuilder
      │
      ▼
   Employee
```

Initially:

```text
Employee
Name       = null
Department = null
Salary     = 0
Email      = null
Address    = null
```

---

### Step 1

```csharp
builder.SetName("Salman");
```

Now:

```text
Employee
Name       = Salman
Department = null
Salary     = 0
Email      = null
Address    = null
```

---

### Step 2

```csharp
builder.SetDepartment("IT");
```

Now:

```text
Employee
Name       = Salman
Department = IT
Salary     = 0
Email      = null
Address    = null
```

---

### Step 3

```csharp
builder.SetSalary(100000);
```

Now:

```text
Employee
Name       = Salman
Department = IT
Salary     = 100000
Email      = null
Address    = null
```

---

### Step 4

```csharp
builder.SetEmail("salman@gmail.com");
```

Now:

```text
Employee
Name       = Salman
Department = IT
Salary     = 100000
Email      = salman@gmail.com
Address    = null
```

---

### Step 5

```csharp
builder.Build();
```

The completed `Employee` is returned.

```text
EmployeeBuilder
       │
       │ configure
       ▼
   Employee
       │
       │ Build()
       ▼
Completed Employee
```

---

# 🏗️ Why Do We Need `Build()`?

`Build()` represents the final step of the construction process.

```csharp
public Employee Build()
{
    return _employee;
}
```

The client does:

```csharp
Employee employee = builder.Build();
```

This gives the client the completed object.

Conceptually:

```text
Configure
   ↓
Configure
   ↓
Configure
   ↓
Build()
   ↓
Completed Object
```

---

# ⚙️ Optional Properties

One of the biggest benefits of Builder is handling optional properties.

Suppose only these fields are required:

```text
Name
Department
Salary
```

while these are optional:

```text
Email
Address
```

We can write:

```csharp
Employee employee = new EmployeeBuilder()
    .SetName("Salman")
    .SetDepartment("IT")
    .SetSalary(100000)
    .Build();
```

No email or address is required.

We can also provide them:

```csharp
Employee employee = new EmployeeBuilder()
    .SetName("Salman")
    .SetDepartment("IT")
    .SetSalary(100000)
    .SetEmail("salman@gmail.com")
    .SetAddress("Karachi")
    .Build();
```

The same builder supports different configurations.

---

# 🔥 Different Configurations

We could create different employees:

### IT Employee

```csharp
Employee employee = new EmployeeBuilder()
    .SetName("Salman")
    .SetDepartment("IT")
    .SetSalary(100000)
    .SetEmail("salman@gmail.com")
    .Build();
```

### HR Employee

```csharp
Employee employee = new EmployeeBuilder()
    .SetName("Ali")
    .SetDepartment("HR")
    .SetSalary(80000)
    .SetAddress("Karachi")
    .Build();
```

Same builder.

Different configuration.

---

# 🖥️ Another Example: Computer

Builder is easier to understand with a complex object such as a computer.

## Product

```csharp
public class Computer
{
    public string CPU { get; set; }
    public int RAM { get; set; }
    public int Storage { get; set; }
    public bool HasGPU { get; set; }
}
```

---

## Builder

```csharp
public class ComputerBuilder
{
    private Computer _computer = new Computer();

    public ComputerBuilder SetCPU(string cpu)
    {
        _computer.CPU = cpu;
        return this;
    }

    public ComputerBuilder SetRAM(int ram)
    {
        _computer.RAM = ram;
        return this;
    }

    public ComputerBuilder SetStorage(int storage)
    {
        _computer.Storage = storage;
        return this;
    }

    public ComputerBuilder AddGPU()
    {
        _computer.HasGPU = true;
        return this;
    }

    public Computer Build()
    {
        return _computer;
    }
}
```

---

# 🎮 Gaming PC

```csharp
Computer gamingPC = new ComputerBuilder()
    .SetCPU("Intel i9")
    .SetRAM(32)
    .SetStorage(1000)
    .AddGPU()
    .Build();
```

Result:

```text
Computer
├── CPU     = Intel i9
├── RAM     = 32 GB
├── Storage = 1000 GB
└── GPU     = Yes
```

---

# 💼 Office PC

We can create another configuration:

```csharp
Computer officePC = new ComputerBuilder()
    .SetCPU("Intel i5")
    .SetRAM(16)
    .SetStorage(512)
    .Build();
```

Result:

```text
Computer
├── CPU     = Intel i5
├── RAM     = 16 GB
├── Storage = 512 GB
└── GPU     = No
```

Same builder.

Different product configuration.

---

# 🆚 Builder vs Factory

This is one of the most important differences to understand.

## Factory

Factory answers:

> **Which object should I create?**

Example:

```csharp
IVehicle vehicle = VehicleFactory.GetVehicle("Car");
```

The Factory might decide between:

```text
Car
Bike
Truck
```

So:

```text
Factory
   ↓
WHICH object?
```

---

## Builder

Builder answers:

> **How should I construct this object?**

Example:

```csharp
Employee employee = new EmployeeBuilder()
    .SetName("Salman")
    .SetDepartment("IT")
    .SetSalary(100000)
    .Build();
```

So:

```text
Builder
   ↓
HOW to build the object?
```

---

# 🧠 Easy Memory Trick

Remember:

```text
Factory → WHICH?
Builder → HOW?
```

### Factory

```text
Factory
   │
   ├── Car
   ├── Bike
   └── Truck
```

The factory decides **which object** to create.

### Builder

```text
Builder
   │
   ├── CPU
   ├── RAM
   ├── Storage
   └── GPU
          ↓
      Computer
```

The builder controls **how the object is configured**.

---

# 🆚 Builder vs Constructor

A constructor is fine when an object is simple:

```csharp
Employee employee = new Employee(
    "Salman",
    "IT",
    100000
);
```

But when there are many parameters:

```csharp
Employee employee = new Employee(
    name,
    department,
    salary,
    email,
    address,
    phone,
    city,
    country,
    // ...
);
```

the code becomes difficult to read and maintain.

Builder gives us:

```csharp
Employee employee = new EmployeeBuilder()
    .SetName("Salman")
    .SetDepartment("IT")
    .SetSalary(100000)
    .SetEmail("salman@gmail.com")
    .SetAddress("Karachi")
    .Build();
```

The intent of every value is clear.

---

# ⭐ Advantages

## 1. Better Readability

Compare:

```csharp
new Employee("Salman", "IT", 100000, "...", "...");
```

with:

```csharp
new EmployeeBuilder()
    .SetName("Salman")
    .SetDepartment("IT")
    .SetSalary(100000)
    .Build();
```

The Builder version clearly communicates what each value represents.

---

## 2. Handles Many Optional Properties

You don't need to provide every property.

```csharp
new EmployeeBuilder()
    .SetName("Salman")
    .SetDepartment("IT")
    .Build();
```

---

## 3. Supports Different Configurations

The same builder can create different versions of the object.

```text
Builder
   │
   ├── Gaming Computer
   ├── Office Computer
   └── Developer Computer
```

---

## 4. Avoids Large Constructors

Instead of:

```csharp
new Employee(a, b, c, d, e, f, g, h, i, j);
```

we have:

```csharp
new EmployeeBuilder()
    .SetName(...)
    .SetDepartment(...)
    .SetSalary(...)
    .Build();
```

---

## 5. Encapsulates Construction Logic

The logic for constructing the object stays inside the builder.

```text
Client
   ↓
Builder
   ↓
Construction Logic
   ↓
Product
```

---

# ⚠️ Possible Disadvantages

### 1. More Classes

A simple object might require:

```text
Employee
EmployeeBuilder
```

instead of just:

```text
Employee
```

For a small object, Builder may be unnecessary.

---

### 2. More Code

You need methods such as:

```csharp
SetName()
SetDepartment()
SetSalary()
SetEmail()
```

This adds code.

---

### 3. Can Be Overkill

If an object only has two or three simple properties, object initialization may be clearer:

```csharp
Employee employee = new Employee
{
    Name = "Salman",
    Department = "IT"
};
```

Builder is most useful when construction is **complex or has many optional/configurable parts**.

---

# 🎯 When Should You Use Builder?

Consider Builder when:

* An object has many properties.
* The object has many optional properties.
* Construction involves multiple steps.
* You want readable object creation.
* You need different configurations of the same object.
* A constructor has become too large or difficult to understand.
* You want to keep construction logic separate from the product.

---

# 🏗️ Builder Pattern Structure

The basic structure can be remembered as:

```text
             Client
                │
                ▼
             Builder
                │
        ┌───────┼────────┐
        ↓       ↓        ↓
      Name    Salary    Email
        │       │        │
        └───────┼────────┘
                ↓
              Build()
                ↓
             Product
```

---

# 🎤 Interview Definition

> **Builder Pattern is a creational design pattern used to construct complex objects step by step. It separates the construction process from the final object, making it easier to create different configurations of the same object.**

---

# ❓ Common Interview Questions

## Q: Why use Builder Pattern?

**Answer:**

> I would use the Builder Pattern when an object has many properties, optional parameters, or complex construction logic. It makes object creation more readable and allows different configurations to be created using the same construction process.

---

## Q: What does `return this` do?

**Answer:**

> `this` refers to the current Builder object. Returning `this` allows builder methods to return the builder itself, which enables method chaining.

Example:

```csharp
public EmployeeBuilder SetName(string name)
{
    _employee.Name = name;
    return this;
}
```

This allows:

```csharp
builder
    .SetName("Salman")
    .SetDepartment("IT")
    .SetSalary(100000);
```

---

## Q: Why do we need `Build()`?

**Answer:**

> `Build()` is the final step that returns the completed product after all required configuration has been applied.

```text
Configure
    ↓
Configure
    ↓
Configure
    ↓
Build()
    ↓
Product
```

---

## Q: Is Builder a Creational or Structural Pattern?

**Answer:**

**Builder is a Creational Design Pattern.**

The three patterns covered so far are:

```text
Creational
└── Builder

Structural
├── Adapter
└── Composite
```

---

# 🧠 How to Identify Builder Pattern

Look for these characteristics:

```text
1. Complex object
       ↓
2. Separate Builder
       ↓
3. Step-by-step configuration
       ↓
4. Methods return the Builder
       ↓
5. Build() returns the final object
```

Typical code:

```csharp
var product = new ProductBuilder()
    .SetPropertyA(...)
    .SetPropertyB(...)
    .SetPropertyC(...)
    .Build();
```

If you see this pattern, you're probably looking at a **Builder**.

---

# 📚 Quick Comparison

| Pattern       | Type       | Main Purpose                                | Memory Trick   |
| ------------- | ---------- | ------------------------------------------- | -------------- |
| **Factory**   | Creational | Creates/selects objects                     | **WHICH?**     |
| **Builder**   | Creational | Builds complex objects step by step         | **HOW?**       |
| **Adapter**   | Structural | Makes incompatible interfaces work together | **TRANSLATOR** |
| **Composite** | Structural | Treats objects and groups uniformly         | **TREE**       |

---

# 🔑 Key Takeaway

Don't memorize the implementation.

Remember these three ideas:

```text
Builder
   ↓
Step-by-step construction
   ↓
Method chaining
   ↓
Build()
```

And the most important distinction:

```text
Factory → WHICH object?
Builder → HOW to build it?
Adapter → HOW can incompatible classes work together?
Composite → HOW can individual objects and groups be treated uniformly?
```

### Final Picture

```text
                 EmployeeBuilder
                       │
          ┌────────────┼────────────┐
          ↓            ↓            ↓
      SetName()   SetDepartment()  SetSalary()
          │            │            │
          └────────────┼────────────┘
                       ↓
                    Build()
                       ↓
                    Employee
```

> **Builder = Build a complex object step by step.**
