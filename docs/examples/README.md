# Membrane Computing Examples

This directory contains comprehensive code examples demonstrating membrane computing patterns in the HOPE framework.

## Available Examples

### 1. BasicMembraneExample.cs
**Difficulty:** Beginner  
**Topics Covered:**
- Creating membrane hierarchies
- Configuring permeability rules
- Moving receptors between membranes
- Dynamic membrane management
- Event handling and monitoring

**Key Learning Points:**
- Understanding the fundamental concepts of membranes and permeability
- How to create and organize membrane structures
- Runtime configuration and adaptation
- Proper cleanup and resource management

### 2. SecurityMembraneExample.cs
**Difficulty:** Intermediate  
**Topics Covered:**
- Multi-zone security architecture
- Strict permeability controls
- Security boundary enforcement
- Runtime security policy changes
- Security auditing and monitoring

**Key Learning Points:**
- Implementing defense in depth with membranes
- Controlling sensitive data flow
- Dynamic security policy adjustment
- Security monitoring and alerting

### 3. LoadBalancingMembraneExample.cs
**Difficulty:** Advanced  
**Topics Covered:**
- Dynamic processor membrane creation
- Automatic load distribution
- System scaling based on load metrics
- Failure detection and recovery
- Real-time system monitoring

**Key Learning Points:**
- Building adaptive, self-scaling systems
- Implementing sophisticated load balancing
- Handling system failures gracefully
- Performance monitoring and optimization

### 4. Program.cs
**Difficulty:** All Levels  
**Purpose:**
- Interactive menu-driven runner for all examples
- Guided exploration of membrane computing concepts
- Help and documentation links
- Educational content about membrane computing

## Getting Started

### Prerequisites
- .NET Framework or .NET Core/.NET 5+
- HOPE framework references
- Basic understanding of C# programming

### Running the Examples

1. **Using the Interactive Runner:**
   ```csharp
   dotnet run Program.cs
   ```
   This provides a menu-driven interface to explore all examples.

2. **Running Individual Examples:**
   ```csharp
   // Basic operations
   BasicMembraneRunner.RunExample();
   
   // Security patterns
   SecurityMembraneRunner.RunExample();
   
   // Load balancing
   LoadBalancingRunner.RunExample();
   ```

### Example Structure

Each example follows a consistent structure:

```csharp
public class ExampleClass
{
    // Setup and configuration methods
    public void CreateSystem() { }
    
    // Demonstration methods
    public void DemonstrateFeatures() { }
    
    // Utility methods
    public void PrintStatus() { }
    
    // Cleanup
    public void Cleanup() { }
}

public class ExampleRunner
{
    public static void RunExample()
    {
        // Complete example execution
    }
}
```

## Key Concepts Demonstrated

### Membrane Hierarchy
```
Root Membrane
├── Processing Layer
│   ├── Data Processors
│   └── Validation Units
├── Security Layer
│   ├── Public Zone
│   ├── Secure Zone
│   └── Crypto Zone
└── Monitoring Layer
    ├── Metrics Collection
    └── Alerting
```

### Permeability Configuration
```csharp
// Allow specific protocols through boundaries
membrane.ProtocolPermeability[new PermeabilityKey 
{ 
    Protocol = "DataRequest", 
    Direction = PermeabilityDirection.In 
}] = new PermeabilityConfiguration { Permeable = true };

// Block sensitive protocols
membrane.ProtocolPermeability[new PermeabilityKey 
{ 
    Protocol = "PrivateKey", 
    Direction = PermeabilityDirection.Out 
}] = new PermeabilityConfiguration { Permeable = false };
```

### Dynamic Adaptation
```csharp
// Runtime reconfiguration based on conditions
if (systemLoad > threshold)
{
    ScaleUpSystem();
}

// Security policy changes
if (securityAlert)
{
    TightenSecurityBoundaries();
}
```

## Learning Path

1. **Start with BasicMembraneExample** to understand fundamental concepts
2. **Progress to SecurityMembraneExample** to see real-world security patterns
3. **Explore LoadBalancingMembraneExample** for advanced dynamic systems
4. **Use Program.cs** for guided exploration and help

## Common Patterns

### Data Processing Pipeline
- Input → Validation → Processing → Output
- Each stage isolated in its own membrane
- Controlled data flow between stages

### Security Boundaries
- Public → Secure → Crypto zones
- Progressively stricter access controls
- Audit trails for sensitive operations

### Dynamic Scaling
- Monitor system metrics
- Create/remove processor membranes as needed
- Load balancing across available resources

## Troubleshooting

### Issue: Receptors Not Communicating
**Solution:** Check permeability configuration - protocols must be allowed through all intermediate membranes.

### Issue: Memory Leaks
**Solution:** Ensure proper cleanup with `membrane.Reset()` and event handler unsubscription.

### Issue: Performance Problems
**Solution:** Avoid deep membrane hierarchies; prefer breadth over depth.

## Integration with HOPE

These examples are designed to work with the HOPE framework but include fallback mechanisms for missing components. In a full HOPE environment:

- Replace mock implementations with actual HOPE components
- Use real receptor assemblies instead of dummy registrations
- Integrate with HOPE's semantic type system
- Leverage HOPE's application controller

## Additional Resources

- [Complete Membrane Computing Tutorial](../MembraneComputingTutorial.md)
- [Quick Start Guide](../MembraneComputingQuickStart.md)
- [HOPE Framework Documentation](../../README.md)
- [Membrane Computing Video](http://youtu.be/XoQSTJcrEj8)

## Contributing

When adding new examples:

1. Follow the established pattern structure
2. Include comprehensive comments
3. Add error handling and cleanup
4. Update this README with the new example
5. Test with both mock and real HOPE components

## License

These examples are provided under the same license as the HOPE framework (GNU GPL V2).