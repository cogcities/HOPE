# Semantic Type System Architecture

## Table of Contents
- [Overview](#overview)
- [Core Concepts](#core-concepts)
- [Type Definition](#type-definition)
- [Code Generation](#code-generation)
- [Runtime System](#runtime-system)
- [Type Relationships](#type-relationships)
- [Persistence Model](#persistence-model)
- [Best Practices](#best-practices)

## Overview

The Semantic Type System (STS) is the foundation of the HOPE architecture. It provides a framework for defining structured data types that carry semantic meaning beyond simple data containers. Unlike traditional type systems that focus on data structure, the STS emphasizes the meaning and relationships of data.

## Core Concepts

```mermaid
graph TB
    subgraph "Semantic Type System Components"
        STD[Semantic Type Declaration]
        SE[Semantic Elements]
        NT[Native Types]
        STN[Semantic Type Nesting]
        UK[Unique Keys]
        REL[Relationships]
    end
    
    subgraph "Processing Pipeline"
        XML[XML Definition]
        PARSE[Parser]
        AST[Abstract Syntax Tree]
        CODEGEN[Code Generator]
        COMPILE[Compiler]
        RUNTIME[Runtime Classes]
    end
    
    STD --> SE
    SE --> NT
    SE --> STN
    SE --> UK
    SE --> REL
    
    STD --> XML
    XML --> PARSE
    PARSE --> AST
    AST --> CODEGEN
    CODEGEN --> COMPILE
    COMPILE --> RUNTIME
```

### Key Principles

1. **Semantic Meaning**: Every type carries semantic information about its purpose and use
2. **Hierarchical Structure**: Types can contain other semantic types, creating rich hierarchies
3. **Unique Identification**: Types can define unique keys for normalization and identity
4. **Relationship Awareness**: Types understand their relationships to other types
5. **Code Generation**: Runtime classes are generated from semantic definitions

## Type Definition

### XML Schema Structure

Semantic types are defined using XML with the following structure:

```xml
<SemanticType Name="TypeName">
  <SemanticElement Name="ElementName" [UniqueKey="true"] [Normalized="true"]>
    <SemanticElement>...</SemanticElement>
    <NativeType Name="PropertyName" Type="DataType" />
  </SemanticElement>
</SemanticType>
```

### Type Definition Architecture

```mermaid
graph TB
    subgraph "Type Definition Hierarchy"
        ST[Semantic Type]
        SE1[Semantic Element 1]
        SE2[Semantic Element 2]
        SE3[Semantic Element 3]
        
        subgraph "Element Contents"
            NST[Nested Semantic Type]
            NT1[Native Type 1]
            NT2[Native Type 2]
        end
    end
    
    ST --> SE1
    ST --> SE2
    ST --> SE3
    SE1 --> NST
    SE2 --> NT1
    SE3 --> NT2
    
    NST -.->|references| ST
```

### Example: RSS Feed Item Type

```xml
<SemanticType Name="RSSFeedItem">
  <!-- Unique identifier for the feed source -->
  <SemanticElement Name="RSSFeedName" UniqueKey="true" Normalized="true">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  
  <!-- Unique URL for the specific item -->
  <SemanticElement Name="RSSFeedUrl" UniqueKey="true" Normalized="true">
    <SemanticElement Name="Url">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  
  <!-- Item content (not unique) -->
  <SemanticElement Name="RSSFeedTitle">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  
  <SemanticElement Name="RSSFeedDescription">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  
  <SemanticElement Name="RSSFeedPubDate">
    <NativeType Name="Value" Type="DateTime" />
  </SemanticElement>
</SemanticType>
```

## Code Generation

### Generation Process

```mermaid
flowchart TD
    subgraph "Input Processing"
        XML[XML Definition]
        VALIDATE[Schema Validation]
        PARSE[Parse to AST]
    end
    
    subgraph "Code Generation"
        ANALYZE[Analyze Dependencies]
        TEMPLATE[Apply Templates]
        GENERATE[Generate C# Code]
    end
    
    subgraph "Output Artifacts"
        CLASSES[C# Classes]
        INTERFACES[Interfaces]
        FACTORIES[Factory Methods]
        METADATA[Type Metadata]
    end
    
    subgraph "Compilation"
        COMPILE[C# Compiler]
        ASSEMBLY[Generated Assembly]
        RUNTIME[Runtime Integration]
    end
    
    XML --> VALIDATE
    VALIDATE --> PARSE
    PARSE --> ANALYZE
    ANALYZE --> TEMPLATE
    TEMPLATE --> GENERATE
    
    GENERATE --> CLASSES
    GENERATE --> INTERFACES
    GENERATE --> FACTORIES
    GENERATE --> METADATA
    
    CLASSES --> COMPILE
    INTERFACES --> COMPILE
    FACTORIES --> COMPILE
    METADATA --> COMPILE
    
    COMPILE --> ASSEMBLY
    ASSEMBLY --> RUNTIME
```

### Generated Class Structure

For the RSSFeedItem example above, the system generates:

```csharp
// Main semantic type class
public class RSSFeedItem : ISemanticType
{
    public RSSFeedName RSSFeedName { get; set; }
    public RSSFeedUrl RSSFeedUrl { get; set; }
    public RSSFeedTitle RSSFeedTitle { get; set; }
    public RSSFeedDescription RSSFeedDescription { get; set; }
    public RSSFeedPubDate RSSFeedPubDate { get; set; }
    
    // Semantic type metadata
    public SemanticTypeStruct Struct { get; }
    public string TypeName => "RSSFeedItem";
    
    // Factory methods
    public static RSSFeedItem Create();
    public static RSSFeedItem Create(RSSFeedName name, RSSFeedUrl url);
}

// Component classes
public class RSSFeedName : ISemanticType
{
    public Text Text { get; set; }
    public string TypeName => "RSSFeedName";
}

public class Text : ISemanticType
{
    public string Value { get; set; }
    public string TypeName => "Text";
}
```

## Runtime System

### Type Registration and Discovery

```mermaid
sequenceDiagram
    participant App as Application
    participant STS as Semantic Type System
    participant TR as Type Registry
    participant CG as Code Generator
    participant RT as Runtime
    
    App->>STS: Load Type Definitions
    STS->>CG: Generate Code
    CG->>RT: Compile Classes
    RT->>TR: Register Types
    TR->>STS: Confirm Registration
    
    Note over App,RT: Type Usage
    App->>STS: Create Instance
    STS->>TR: Lookup Type
    TR->>STS: Return Type Info
    STS->>App: Return Instance
```

### Type Instance Creation

```mermaid
graph LR
    subgraph "Instance Creation Flow"
        REQ[Creation Request]
        LOOKUP[Type Lookup]
        FACTORY[Factory Method]
        CONSTRUCT[Object Construction]
        INIT[Initialization]
        VALIDATE[Validation]
        INSTANCE[Type Instance]
    end
    
    REQ --> LOOKUP
    LOOKUP --> FACTORY
    FACTORY --> CONSTRUCT
    CONSTRUCT --> INIT
    INIT --> VALIDATE
    VALIDATE --> INSTANCE
    
    VALIDATE -.->|validation error| REQ
```

## Type Relationships

### Inheritance and Composition

```mermaid
classDiagram
    class ISemanticType {
        <<interface>>
        +TypeName: string
        +Struct: SemanticTypeStruct
    }
    
    class SemanticTypeBase {
        <<abstract>>
        +TypeName: string
        +Struct: SemanticTypeStruct
        +Validate(): bool
        +Clone(): ISemanticType
    }
    
    class Text {
        +Value: string
    }
    
    class Url {
        +Value: string
        +IsValid(): bool
    }
    
    class RSSFeedUrl {
        +Url: Url
    }
    
    class RSSFeedName {
        +Text: Text
    }
    
    class RSSFeedItem {
        +RSSFeedName: RSSFeedName
        +RSSFeedUrl: RSSFeedUrl
        +RSSFeedTitle: Text
        +RSSFeedDescription: Text
        +RSSFeedPubDate: DateTime
    }
    
    ISemanticType <|-- SemanticTypeBase
    SemanticTypeBase <|-- Text
    SemanticTypeBase <|-- Url
    SemanticTypeBase <|-- RSSFeedUrl
    SemanticTypeBase <|-- RSSFeedName
    SemanticTypeBase <|-- RSSFeedItem
    
    RSSFeedUrl --> Url
    RSSFeedName --> Text
    RSSFeedItem --> RSSFeedName
    RSSFeedItem --> RSSFeedUrl
```

### Type Dependencies

```mermaid
graph TB
    subgraph "Complex Type Dependencies"
        subgraph "Application Types"
            WEATHER[WeatherInfo]
            LOCATION[LocationInfo]
            FORECAST[WeatherForecast]
        end
        
        subgraph "Common Types"
            TEXT[Text]
            NUMBER[Number]
            DATE[DateTime]
            URL[Url]
        end
        
        subgraph "Geographic Types"
            GEO[GeoCoordinate]
            ADDR[Address]
            ZIP[ZipCode]
        end
    end
    
    WEATHER --> LOCATION
    WEATHER --> NUMBER
    WEATHER --> DATE
    FORECAST --> WEATHER
    LOCATION --> GEO
    LOCATION --> ADDR
    ADDR --> TEXT
    ADDR --> ZIP
    GEO --> NUMBER
```

## Persistence Model

### Database Schema Generation

The semantic type system automatically generates database schemas that reflect the semantic structure:

```mermaid
erDiagram
    RSSFeedItem {
        int ID PK
        int RSSFeedName_ID FK
        int RSSFeedUrl_ID FK
        int RSSFeedTitle_ID FK
        int RSSFeedDescription_ID FK
        datetime RSSFeedPubDate
    }
    
    RSSFeedName {
        int ID PK
        int Text_ID FK
    }
    
    RSSFeedUrl {
        int ID PK
        int Url_ID FK
    }
    
    Text {
        int ID PK
        string Value
    }
    
    Url {
        int ID PK
        string Value
    }
    
    RSSFeedItem ||--|| RSSFeedName : contains
    RSSFeedItem ||--|| RSSFeedUrl : contains
    RSSFeedName ||--|| Text : contains
    RSSFeedUrl ||--|| Url : contains
```

### Normalization Strategy

```mermaid
graph TB
    subgraph "Normalization Process"
        subgraph "Input Data"
            RAW[Raw Semantic Data]
            KEYS[Unique Keys]
            VALUES[Value Data]
        end
        
        subgraph "Normalization"
            CHECK[Check Existing]
            NORM[Normalize Tables]
            REF[Create References]
        end
        
        subgraph "Storage"
            TABLES[Normalized Tables]
            REFS[Reference Keys]
            CACHE[Memory Cache]
        end
    end
    
    RAW --> KEYS
    RAW --> VALUES
    KEYS --> CHECK
    CHECK -->|exists| REFS
    CHECK -->|new| NORM
    VALUES --> NORM
    NORM --> TABLES
    NORM --> REFS
    TABLES --> CACHE
    REFS --> CACHE
```

Key principles:
1. **Semantic Tables**: Each semantic type becomes a database table
2. **Foreign Key Structure**: Parent types reference child types via foreign keys
3. **Normalization**: Unique/normalized types prevent data duplication
4. **Thin Tables**: Higher-level tables consist mainly of foreign key references
5. **Native Type Storage**: Only bottom-level tables store actual native type values

## Best Practices

### Type Design Guidelines

```mermaid
graph LR
    subgraph "Design Principles"
        MEANING[Semantic Meaning]
        REUSE[Reusability]
        COMPOSIT[Composition]
        UNIQUE[Uniqueness]
        NORMAL[Normalization]
    end
    
    subgraph "Implementation"
        STRUCT[Structure Design]
        NAMING[Naming Convention]
        RELATIONSHIPS[Relationship Design]
        VALIDATION[Validation Rules]
    end
    
    MEANING --> STRUCT
    REUSE --> NAMING
    COMPOSIT --> RELATIONSHIPS
    UNIQUE --> VALIDATION
    NORMAL --> VALIDATION
```

### 1. Semantic Meaning First
- Choose type names that reflect business meaning, not technical structure
- Example: `CustomerAddress` not `StringCollection`

### 2. Promote Reusability
- Create common types like `Text`, `Url`, `EmailAddress` for reuse across multiple semantic types
- Avoid duplicating structure in different types

### 3. Design for Composition
- Build complex types from simpler semantic elements
- Use nesting to create hierarchical meaning

### 4. Define Appropriate Uniqueness
- Mark semantic elements as `UniqueKey="true"` when they serve as identifiers
- Use `Normalized="true"` for types that should be deduplicated in storage

### 5. Consider Persistence Implications
- Understand that each semantic type becomes a database table
- Design with the resulting foreign key relationships in mind

### Example: Well-Designed Type

```xml
<!-- Good: Semantic meaning, reusable components, appropriate uniqueness -->
<SemanticType Name="CustomerOrder">
  <SemanticElement Name="OrderNumber" UniqueKey="true" Normalized="true">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  
  <SemanticElement Name="Customer">
    <SemanticElement Name="CustomerID" UniqueKey="true" Normalized="true">
      <NativeType Name="Value" Type="int" />
    </SemanticElement>
    <SemanticElement Name="CustomerName">
      <SemanticElement Name="Text">
        <NativeType Name="Value" Type="string" />
      </SemanticElement>
    </SemanticElement>
  </SemanticElement>
  
  <SemanticElement Name="OrderDate">
    <NativeType Name="Value" Type="DateTime" />
  </SemanticElement>
  
  <SemanticElement Name="TotalAmount">
    <SemanticElement Name="Currency">
      <NativeType Name="Amount" Type="decimal" />
      <NativeType Name="CurrencyCode" Type="string" />
    </SemanticElement>
  </SemanticElement>
</SemanticType>
```

## Related Documentation

- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Overall system architecture
- **[Receptor-Architecture.md](Receptor-Architecture.md)** - How receptors use semantic types
- **[Data-Flow.md](Data-Flow.md)** - How semantic data flows through the system
- **[Examples.md](Examples.md)** - Practical examples of semantic type usage