# HOPE Technical Architecture

## Table of Contents
- [Overview](#overview)
- [Core Concepts](#core-concepts)
- [System Architecture](#system-architecture)
- [Component Architecture](#component-architecture)
- [Semantic Type System](#semantic-type-system)
- [Receptor Architecture](#receptor-architecture)
- [Communication Patterns](#communication-patterns)
- [Application Stack](#application-stack)
- [Related Documentation](#related-documentation)

## Overview

The Higher Order Programming Environment (HOPE) is an architectural framework for implementing end-user processes as finite automata (FA) in a distributed computing space. The system enables building applications by composing "receptors" that communicate through semanticized data in a publish-subscribe pattern.

### Key Principles

- **Semantic-First Design**: All data is semantically typed, enabling rich meaning and automatic interconnection
- **Receptor-Based Components**: Applications are built from finite automata receptors that self-wire based on semantic interest
- **Emergent Architecture**: New computational stacks emerge from combining existing receptors with new semantic types
- **Dynamic Composition**: Receptors can be added, removed, or reconfigured at runtime

## Core Concepts

```mermaid
graph TB
    subgraph "Core Concepts"
        ST[Semantic Types]
        R[Receptors]
        STS[Semantic Type System]
        B[Broker/Membrane]
    end
    
    ST --> R
    STS --> ST
    R --> B
    B --> R
    
    ST -.->|defines structure| STS
    R -.->|publishes/subscribes| ST
    B -.->|routes semantic data| R
```

### Semantic Types
Structured data definitions that carry meaning beyond simple data types. Examples:
- `RSSFeedItem` containing `Title`, `Url`, `Description`, `PubDate`
- `WeatherInfo` containing `Temperature`, `Humidity`, `Location`
- `ImageInfo` containing `Filename`, `Dimensions`, `Format`

### Receptors
Self-contained computational units that:
- Subscribe to specific semantic types
- Process received data
- Emit new semantic types
- Can maintain internal state

### Membrane/Broker
Central communication hub that:
- Routes semantic data between receptors
- Manages receptor lifecycle
- Handles semantic type registration
- Provides filtering and transformation capabilities

## System Architecture

```mermaid
graph TB
    subgraph "HOPE System Architecture"
        subgraph "Application Layer"
            UI[User Interface]
            APP[Application Logic]
        end
        
        subgraph "Receptor Layer"
            R1[Input Receptors]
            R2[Processing Receptors]
            R3[Output Receptors]
            R4[UI Receptors]
        end
        
        subgraph "Communication Layer"
            M[Membrane/Broker]
            STR[Semantic Type Registry]
        end
        
        subgraph "Core Layer"
            STS[Semantic Type System]
            RB[Receptor Base Classes]
            PS[Persistence System]
        end
        
        subgraph "Infrastructure Layer"
            NET[.NET Framework]
            WIN[Windows Forms]
            EXT[External Services]
        end
    end
    
    UI --> R4
    APP --> R1
    R1 --> M
    R2 --> M
    R3 --> M
    R4 --> M
    M --> STR
    M --> R1
    M --> R2
    M --> R3
    M --> R4
    STR --> STS
    R1 --> RB
    R2 --> RB
    R3 --> RB
    R4 --> RB
    RB --> STS
    M --> PS
    STS --> NET
    RB --> NET
    R4 --> WIN
    R1 --> EXT
```

## Component Architecture

```mermaid
graph LR
    subgraph "Core Libraries"
        STSI[Clifton.SemanticTypeSystem.Interfaces]
        STS[Clifton.SemanticTypeSystem]
        RI[Clifton.Receptor.Interfaces]
        R[Clifton.Receptor]
    end
    
    subgraph "UI Framework"
        DW[Clifton.DockableWindows]
        WF[Clifton.Windows.Forms]
    end
    
    subgraph "Application Framework"
        HI[Hope.Interfaces]
        TSE[TypeSystemExplorer]
    end
    
    subgraph "Receptor Ecosystem"
        subgraph "Data Input"
            RSS[RSS Receptors]
            WS[Web Scraping Receptors]
            WEB[Web Service Receptors]
        end
        
        subgraph "Processing"
            NLP[NLP Receptors]
            FILTER[Filter Receptors]
            PERSIST[Persistence Receptors]
        end
        
        subgraph "Output"
            UI_R[UI Receptors]
            FILE[File Output Receptors]
            SPEECH[Text-to-Speech Receptors]
        end
        
        subgraph "Fun & Games"
            GAME[Game Receptors]
            DEMO[Demo Receptors]
        end
    end
    
    STS --> STSI
    R --> RI
    R --> STS
    WF --> DW
    TSE --> STS
    TSE --> R
    HI --> RI
    
    RSS --> R
    WS --> R
    WEB --> R
    NLP --> R
    FILTER --> R
    PERSIST --> R
    UI_R --> R
    UI_R --> WF
    FILE --> R
    SPEECH --> R
    GAME --> R
    DEMO --> R
```

## Semantic Type System

```mermaid
graph TB
    subgraph "Semantic Type Definition"
        STD[Semantic Type Declaration]
        SE[Semantic Elements]
        NT[Native Types]
        ST[Sub Semantic Types]
    end
    
    subgraph "Type System Components"
        PARSER[XML Parser]
        COMPILER[Code Compiler]
        CODEGEN[Code Generator]
        RUNTIME[Runtime System]
    end
    
    subgraph "Generated Artifacts"
        CLASSES[C# Classes]
        INTERFACES[Interfaces]
        FACTORIES[Factory Methods]
    end
    
    STD --> PARSER
    PARSER --> SE
    SE --> NT
    SE --> ST
    PARSER --> COMPILER
    COMPILER --> CODEGEN
    CODEGEN --> CLASSES
    CODEGEN --> INTERFACES
    CODEGEN --> FACTORIES
    CLASSES --> RUNTIME
    INTERFACES --> RUNTIME
    FACTORIES --> RUNTIME
    
    NT -.->|string, int, bool, etc.| CLASSES
    ST -.->|nested semantic types| CLASSES
```

### Semantic Type Example

```xml
<SemanticType Name="RSSFeedItem">
  <SemanticElement Name="RSSFeedName" UniqueKey="true">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="RSSFeedUrl" UniqueKey="true">
    <SemanticElement Name="Url">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
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

## Receptor Architecture

```mermaid
graph TB
    subgraph "Receptor Base Architecture"
        BR[BaseReceptor]
        WBR[WindowedBaseReceptor]
        EP[EmittedProtocol]
        RQ[ReceiveQualifier]
    end
    
    subgraph "Receptor Implementation"
        IMPL[Concrete Receptor]
        SUBS[Subscriptions]
        PROC[Processing Logic]
        EMIT[Emission Logic]
    end
    
    subgraph "Membrane Interface"
        REG[Registration]
        SUB[Subscribe]
        PUB[Publish]
        FILTER[Filtering]
    end
    
    BR --> IMPL
    WBR --> IMPL
    IMPL --> SUBS
    IMPL --> PROC
    IMPL --> EMIT
    EP --> EMIT
    RQ --> FILTER
    
    SUBS --> SUB
    EMIT --> PUB
    IMPL --> REG
    SUB --> FILTER
```

### Receptor Lifecycle

```mermaid
sequenceDiagram
    participant M as Membrane
    participant R as Receptor
    participant STS as Semantic Type System
    
    Note over R: Receptor Creation
    R->>M: Register with Membrane
    R->>STS: Declare Semantic Interests
    M->>R: Acknowledge Registration
    
    Note over R: Runtime Operation
    loop Data Processing
        M->>R: Emit Semantic Data
        R->>R: Process Data
        R->>M: Publish Results
        M->>M: Route to Interested Receptors
    end
    
    Note over R: Shutdown
    R->>M: Unregister
    M->>R: Acknowledge Unregistration
```

## Communication Patterns

### Publish-Subscribe Pattern

```mermaid
graph LR
    subgraph "Publishers"
        P1[RSS Reader]
        P2[Web Scraper]
        P3[Timer]
    end
    
    subgraph "Membrane"
        M[Message Broker]
        F[Semantic Filter]
        R[Router]
    end
    
    subgraph "Subscribers"
        S1[UI Display]
        S2[Persistence]
        S3[NLP Processor]
        S4[Logger]
    end
    
    P1 -->|RSSFeedItem| M
    P2 -->|WebPageContent| M
    P3 -->|TimerEvent| M
    
    M --> F
    F --> R
    
    R -->|Filtered Data| S1
    R -->|All Data| S2
    R -->|Text Content| S3
    R -->|All Events| S4
```

### Semantic Data Flow

```mermaid
flowchart TD
    subgraph "Input Layer"
        RSS[RSS Feed Reader]
        WEB[Web Page Scraper]
        USER[User Input]
    end
    
    subgraph "Processing Layer"
        FILTER[Content Filter]
        NLP[NLP Processor]
        TRANSFORM[Data Transformer]
    end
    
    subgraph "Storage Layer"
        DB[Database Persistence]
        CACHE[Memory Cache]
        FILE[File Storage]
    end
    
    subgraph "Output Layer"
        UI[UI Display]
        REPORT[Report Generator]
        EXPORT[Data Export]
    end
    
    RSS -->|RSSFeedItem| FILTER
    WEB -->|WebPageContent| FILTER
    USER -->|UserInput| TRANSFORM
    
    FILTER -->|FilteredContent| NLP
    FILTER -->|ValidatedData| DB
    NLP -->|ProcessedText| TRANSFORM
    
    DB -->|StoredData| UI
    TRANSFORM -->|FormattedData| UI
    CACHE -->|CachedData| UI
    
    DB -->|QueryResults| REPORT
    TRANSFORM -->|ProcessedData| EXPORT
    FILE -->|FileData| EXPORT
```

## Application Stack

### Example: APOD (Astronomy Picture of the Day) Application

```mermaid
graph TB
    subgraph "APOD Application Stack"
        subgraph "Data Sources"
            APOD_WS[APOD Web Service]
            APOD_SCRAPER[APOD Scraper Receptor]
        end
        
        subgraph "Processing"
            IMG_DL[Image Downloader]
            THUMB[Thumbnail Creator]
            META[Metadata Extractor]
        end
        
        subgraph "Storage"
            IMG_STORE[Image Storage]
            META_DB[Metadata Database]
        end
        
        subgraph "User Interface"
            IMG_VIEWER[Image Viewer]
            THUMB_VIEWER[Thumbnail Viewer]
            INFO_DISPLAY[Info Display]
        end
        
        subgraph "Utilities"
            TIMER[Daily Timer]
            LOGGER[Event Logger]
        end
    end
    
    APOD_WS --> APOD_SCRAPER
    APOD_SCRAPER -->|APODInfo| IMG_DL
    APOD_SCRAPER -->|APODInfo| META
    IMG_DL -->|ImageFile| THUMB
    IMG_DL -->|ImageFile| IMG_STORE
    THUMB -->|ThumbnailImage| THUMB_VIEWER
    META -->|MetadataInfo| META_DB
    META -->|MetadataInfo| INFO_DISPLAY
    IMG_STORE -->|StoredImage| IMG_VIEWER
    META_DB -->|QueryResults| INFO_DISPLAY
    TIMER -->|TimerEvent| APOD_SCRAPER
    APOD_SCRAPER -->|LogEvent| LOGGER
    IMG_DL -->|LogEvent| LOGGER
```

### Semantic Types in APOD Application

```mermaid
classDiagram
    class APODInfo {
        +Title: Text
        +Explanation: Text
        +Url: ImageUrl
        +HdUrl: ImageUrl
        +Date: DateTime
        +MediaType: Text
        +Copyright: Text
    }
    
    class ImageUrl {
        +Value: string
    }
    
    class Text {
        +Value: string
    }
    
    class ImageFile {
        +Filename: Text
        +Data: binary
        +Size: Integer
        +Format: Text
    }
    
    class ThumbnailImage {
        +OriginalImage: ImageFile
        +ThumbnailData: binary
        +Width: Integer
        +Height: Integer
    }
    
    class MetadataInfo {
        +Source: APODInfo
        +ProcessedDate: DateTime
        +Tags: TextList
        +Category: Text
    }
    
    APODInfo --> ImageUrl
    APODInfo --> Text
    ImageFile --> Text
    ThumbnailImage --> ImageFile
    MetadataInfo --> APODInfo
    MetadataInfo --> Text
```

## Related Documentation

- **[Semantic Type System](Semantic-Type-System.md)** - Detailed semantic type system architecture
- **[Receptor Architecture](Receptor-Architecture.md)** - In-depth receptor design patterns
- **[Data Flow](Data-Flow.md)** - Communication patterns and data routing
- **[Examples](Examples.md)** - Practical implementation examples
- **[Deployment](Deployment.md)** - Setup and deployment instructions

## External Resources

- [Main Project Repository](https://github.com/rzonedevops/HOPE)
- [HOPE Introduction Video](http://youtu.be/O1V4XSYYNxs)
- [APOD Scraper Demo](http://youtu.be/NdapAL2tt7w)
- [Membrane Computing Video](http://youtu.be/XoQSTJcrEj8)
- [CodeProject Articles](http://www.codeproject.com/Articles/777843/HOPE-Higher-Order-Programming-Environment)