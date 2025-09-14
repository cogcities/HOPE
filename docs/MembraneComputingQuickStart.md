# Membrane Computing Quick Start Guide

This is a condensed version of the complete [Membrane Computing Tutorial](MembraneComputingTutorial.md) to get you up and running quickly.

## What You Need to Know

**Membrane Computing** in HOPE is like having smart containers that control what information can flow in and out. Think of it like cellular membranes that control molecular transport.

## 5-Minute Example

Here's how to create your first membrane system:

### Step 1: Create a Basic Membrane
```csharp
// Setup the foundation
ISemanticTypeSystem sts = new SemanticTypeSystem();
IApplicationController appController = new ApplicationController();

// Create the root membrane
Membrane rootMembrane = new Membrane(sts, appController);
rootMembrane.Name = "MyRootMembrane";
```

### Step 2: Add Child Membranes
```csharp
// Create specialized membranes for different purposes
Membrane dataProcessing = rootMembrane.CreateInnerMembrane();
dataProcessing.Name = "DataProcessing";

Membrane userInterface = rootMembrane.CreateInnerMembrane();
userInterface.Name = "UserInterface";
```

### Step 3: Add Receptors
```csharp
// Add receptors to the membranes
dataProcessing.RegisterReceptor("DataProcessor", "DataProcessor.dll");
userInterface.RegisterReceptor("UIDisplay", "UIDisplay.dll");
```

### Step 4: Configure What Can Pass Through
```csharp
// Configure permeability - what protocols can cross membrane boundaries
var dataOutKey = new PermeabilityKey 
{ 
    Protocol = "ProcessedData", 
    Direction = PermeabilityDirection.Out 
};
dataProcessing.ProtocolPermeability[dataOutKey] = 
    new PermeabilityConfiguration { Permeable = true };

var dataInKey = new PermeabilityKey 
{ 
    Protocol = "ProcessedData", 
    Direction = PermeabilityDirection.In 
};
userInterface.ProtocolPermeability[dataInKey] = 
    new PermeabilityConfiguration { Permeable = true };
```

## Common Patterns

### 1. Security Boundary
```csharp
var secureMembrane = root.CreateInnerMembrane();
secureMembrane.Name = "SecureZone";

// Only allow encrypted data in and out
secureMembrane.ProtocolPermeability[new PermeabilityKey 
{ 
    Protocol = "EncryptedData", 
    Direction = PermeabilityDirection.In 
}] = new PermeabilityConfiguration { Permeable = true };

// Block plain text from leaving
secureMembrane.ProtocolPermeability[new PermeabilityKey 
{ 
    Protocol = "PlainText", 
    Direction = PermeabilityDirection.Out 
}] = new PermeabilityConfiguration { Permeable = false };
```

### 2. Processing Pipeline
```csharp
var input = root.CreateInnerMembrane();
input.Name = "Input";
var processing = root.CreateInnerMembrane();
processing.Name = "Processing";
var output = root.CreateInnerMembrane();
output.Name = "Output";

// Input → Processing
input.ProtocolPermeability[new PermeabilityKey 
{ 
    Protocol = "RawData", 
    Direction = PermeabilityDirection.Out 
}] = new PermeabilityConfiguration { Permeable = true };

processing.ProtocolPermeability[new PermeabilityKey 
{ 
    Protocol = "RawData", 
    Direction = PermeabilityDirection.In 
}] = new PermeabilityConfiguration { Permeable = true };

// Processing → Output
processing.ProtocolPermeability[new PermeabilityKey 
{ 
    Protocol = "ProcessedData", 
    Direction = PermeabilityDirection.Out 
}] = new PermeabilityConfiguration { Permeable = true };

output.ProtocolPermeability[new PermeabilityKey 
{ 
    Protocol = "ProcessedData", 
    Direction = PermeabilityDirection.In 
}] = new PermeabilityConfiguration { Permeable = true };
```

## XML Configuration Alternative

Instead of code, you can configure membranes with XML:

```xml
<MembraneDef Name="DataProcessing">
  <Receptors>
    <ReceptorDef Name="DataProcessor" AssemblyName="DataProcessor.dll" Enabled="True" />
  </Receptors>
  <Permeabilities>
    <PermeabilityDef Protocol="RawData" Direction="In" Permeable="True" />
    <PermeabilityDef Protocol="ProcessedData" Direction="Out" Permeable="True" />
    <PermeabilityDef Protocol="InternalState" Direction="Out" Permeable="False" />
  </Permeabilities>
</MembraneDef>
```

## Key Concepts

- **Membrane**: Container for receptors and other membranes
- **Receptor**: Processing unit that handles specific protocols
- **Protocol**: Typed message that flows between receptors
- **Permeability**: Rules about what protocols can cross membrane boundaries
- **Direction**: `In` (into membrane) or `Out` (out of membrane)

## Debugging Tips

```csharp
// Check what's happening
public void DebugMembrane(Membrane membrane)
{
    Console.WriteLine($"Membrane: {membrane.Name}");
    Console.WriteLine($"Receptors: {membrane.Receptors.Count}");
    Console.WriteLine($"Child Membranes: {membrane.Membranes.Count}");
    
    // Check permeability
    foreach (var kvp in membrane.ProtocolPermeability)
    {
        Console.WriteLine($"  {kvp.Key.Protocol} ({kvp.Key.Direction}): {kvp.Value.Permeable}");
    }
}
```

## Next Steps

1. **Try the Examples**: Load and run the existing applets in `/Applets/membrane computing`
2. **Read the Full Tutorial**: See [MembraneComputingTutorial.md](MembraneComputingTutorial.md) for comprehensive coverage
3. **Watch the Video**: [Membrane Computing Video](http://youtu.be/XoQSTJcrEj8)
4. **Experiment**: Create your own membrane configurations and see how they behave

## Common Mistakes to Avoid

1. **Forgetting to set permeability**: If protocols aren't crossing boundaries, check your permeability configuration
2. **Not calling UpdatePermeability()**: After making changes, call this method
3. **Creating circular dependencies**: Avoid membrane configurations that can cause deadlocks
4. **Going too deep**: Prefer breadth over depth in membrane hierarchies for better performance

---

*This is a quick introduction. For complete documentation, examples, and advanced patterns, see the [Complete Membrane Computing Tutorial](MembraneComputingTutorial.md).*