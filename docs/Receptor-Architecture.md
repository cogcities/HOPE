# Receptor Architecture

## Table of Contents
- [Overview](#overview)
- [Receptor Fundamentals](#receptor-fundamentals)
- [Base Receptor Architecture](#base-receptor-architecture)
- [Receptor Types](#receptor-types)
- [Communication Patterns](#communication-patterns)
- [Lifecycle Management](#lifecycle-management)
- [Implementation Patterns](#implementation-patterns)
- [Best Practices](#best-practices)

## Overview

Receptors are the fundamental computational units in the HOPE architecture. They are self-contained finite automata that process semantic data, maintain state, and communicate through a publish-subscribe mechanism. Each receptor specializes in a specific computational task and can be dynamically composed with other receptors to create complex applications.

## Receptor Fundamentals

```mermaid
graph TB
    subgraph "Receptor Core Concepts"
        subgraph "Input"
            SUB[Subscriptions]
            FILTER[Filters]
            QUAL[Qualifiers]
        end
        
        subgraph "Processing"
            STATE[Internal State]
            LOGIC[Business Logic]
            TRANSFORM[Data Transformation]
        end
        
        subgraph "Output"
            EMIT[Emissions]
            PROTOCOL[Protocols]
            RESPONSE[Response Types]
        end
        
        subgraph "Infrastructure"
            MEMBRANE[Membrane Interface]
            LIFECYCLE[Lifecycle Management]
            CONFIG[Configuration]
        end
    end
    
    SUB --> LOGIC
    FILTER --> LOGIC
    QUAL --> LOGIC
    STATE --> LOGIC
    LOGIC --> TRANSFORM
    TRANSFORM --> EMIT
    EMIT --> PROTOCOL
    PROTOCOL --> RESPONSE
    
    MEMBRANE --> SUB
    MEMBRANE --> EMIT
    LIFECYCLE --> STATE
    CONFIG --> LOGIC
```

### Key Characteristics

1. **Autonomous Operation**: Receptors operate independently, responding to semantic data
2. **Semantic Awareness**: Subscribe to and emit specific semantic types
3. **State Management**: Can maintain internal state between processing cycles
4. **Dynamic Filtering**: Can qualify which semantic data to process based on content and context
5. **Composable**: Can be combined with other receptors to create complex applications

## Base Receptor Architecture

```mermaid
classDiagram
    class IReceptor {
        <<interface>>
        +Name: string
        +IsEnabled: bool
        +Initialize()
        +ProcessCarrier(carrier)
        +Terminate()
    }
    
    class BaseReceptor {
        <<abstract>>
        +Name: string
        +IsEnabled: bool
        +Membrane: IMembrane
        +ProcessCarrier(carrier)
        #ProcessMessage(message)
        #Emit(semanticType)
        #Subscribe(type, qualifier)
    }
    
    class WindowedBaseReceptor {
        <<abstract>>
        +Form: Form
        +CreateForm()
        +ShowForm()
        +HideForm()
        +CloseForm()
    }
    
    class ConcreteReceptor {
        +ProcessMessage(message)
        +Initialize()
        +Terminate()
    }
    
    IReceptor <|-- BaseReceptor
    BaseReceptor <|-- WindowedBaseReceptor
    BaseReceptor <|-- ConcreteReceptor
    WindowedBaseReceptor <|-- ConcreteReceptor
```

### Core Components

#### BaseReceptor
```csharp
public abstract class BaseReceptor : IReceptor
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    protected IMembrane Membrane { get; set; }
    
    // Subscribe to semantic types
    protected void Subscribe<T>() where T : ISemanticType
    protected void Subscribe<T>(ReceiveQualifier qualifier) where T : ISemanticType
    
    // Emit semantic types
    protected void Emit<T>(T semanticType) where T : ISemanticType
    
    // Process incoming messages
    protected abstract void ProcessMessage(ICarrier carrier);
}
```

#### Subscription and Emission

```mermaid
sequenceDiagram
    participant R as Receptor
    participant M as Membrane
    participant ST as Semantic Type
    participant OR as Other Receptors
    
    Note over R,M: Subscription Phase
    R->>M: Subscribe<RSSFeedItem>()
    M->>R: Acknowledge Subscription
    
    Note over R,M: Runtime Processing
    OR->>M: Emit(RSSFeedItem)
    M->>M: Route to Subscribers
    M->>R: ProcessCarrier(carrier)
    R->>R: ProcessMessage(message)
    R->>M: Emit(ProcessedData)
    M->>OR: Route ProcessedData
```

## Receptor Types

### 1. Input Receptors

Input receptors bring external data into the HOPE system by reading from external sources and emitting semantic types.

```mermaid
graph LR
    subgraph "Input Receptors"
        RSS[RSS Feed Reader]
        WEB[Web Page Scraper]
        FILE[File Reader]
        TIMER[Timer Receptor]
        USER[User Input Receptor]
        API[API Client Receptor]
    end
    
    subgraph "External Sources"
        FEED[RSS Feeds]
        SITE[Web Sites]
        FS[File System]
        CLOCK[System Clock]
        UI[User Interface]
        SERVICE[Web Services]
    end
    
    subgraph "Semantic Output"
        RSS_ST[RSSFeedItem]
        WEB_ST[WebPageContent]
        FILE_ST[FileContent]
        TIME_ST[TimerEvent]
        INPUT_ST[UserInput]
        API_ST[APIResponse]
    end
    
    FEED --> RSS
    SITE --> WEB
    FS --> FILE
    CLOCK --> TIMER
    UI --> USER
    SERVICE --> API
    
    RSS --> RSS_ST
    WEB --> WEB_ST
    FILE --> FILE_ST
    TIMER --> TIME_ST
    USER --> INPUT_ST
    API --> API_ST
```

#### Example: RSS Feed Reader Receptor

```csharp
public class RSSFeedReaderReceptor : BaseReceptor
{
    private Timer feedTimer;
    private string feedUrl;
    
    public override void Initialize()
    {
        Subscribe<RSSFeedRequest>();
        feedTimer = new Timer(CheckFeed, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
    }
    
    protected override void ProcessMessage(ICarrier carrier)
    {
        if (carrier.Message is RSSFeedRequest request)
        {
            feedUrl = request.Url.Value;
            CheckFeed(null);
        }
    }
    
    private void CheckFeed(object state)
    {
        try
        {
            var feed = LoadRSSFeed(feedUrl);
            foreach (var item in feed.Items)
            {
                var rssItem = CreateRSSFeedItem(item);
                Emit(rssItem);
            }
        }
        catch (Exception ex)
        {
            Emit(new ExceptionInfo { Message = ex.Message });
        }
    }
}
```

### 2. Processing Receptors

Processing receptors transform, filter, aggregate, or analyze semantic data.

```mermaid
graph TB
    subgraph "Processing Receptors"
        FILTER[Filter Receptor]
        TRANSFORM[Transform Receptor]
        AGGREGATE[Aggregation Receptor]
        NLP[NLP Processor]
        VALIDATE[Validation Receptor]
        ENRICH[Data Enrichment]
    end
    
    subgraph "Input Types"
        RAW[Raw Data]
        TEXT[Text Content]
        ITEMS[Data Items]
        STRUCTURED[Structured Data]
    end
    
    subgraph "Output Types"
        FILTERED[Filtered Data]
        TRANSFORMED[Transformed Data]
        SUMMARY[Aggregated Summary]
        ANALYZED[Analysis Results]
        VALIDATED[Validated Data]
        ENRICHED[Enriched Data]
    end
    
    RAW --> FILTER
    TEXT --> NLP
    ITEMS --> AGGREGATE
    STRUCTURED --> TRANSFORM
    RAW --> VALIDATE
    STRUCTURED --> ENRICH
    
    FILTER --> FILTERED
    NLP --> ANALYZED
    AGGREGATE --> SUMMARY
    TRANSFORM --> TRANSFORMED
    VALIDATE --> VALIDATED
    ENRICH --> ENRICHED
```

#### Example: Content Filter Receptor

```csharp
public class ContentFilterReceptor : BaseReceptor
{
    private List<string> keywords;
    private FilterMode mode;
    
    public override void Initialize()
    {
        Subscribe<RSSFeedItem>();
        Subscribe<FilterConfiguration>();
    }
    
    protected override void ProcessMessage(ICarrier carrier)
    {
        switch (carrier.Message)
        {
            case FilterConfiguration config:
                keywords = config.Keywords;
                mode = config.Mode;
                break;
                
            case RSSFeedItem item:
                if (ShouldInclude(item))
                {
                    Emit(new FilteredContent
                    {
                        OriginalItem = item,
                        FilterReason = "Keyword match",
                        FilteredAt = DateTime.Now
                    });
                }
                break;
        }
    }
    
    private bool ShouldInclude(RSSFeedItem item)
    {
        var content = $"{item.RSSFeedTitle.Text.Value} {item.RSSFeedDescription.Text.Value}";
        
        return mode == FilterMode.Include
            ? keywords.Any(k => content.Contains(k, StringComparison.OrdinalIgnoreCase))
            : !keywords.Any(k => content.Contains(k, StringComparison.OrdinalIgnoreCase));
    }
}
```

### 3. Output Receptors

Output receptors present data to users or export data to external systems.

```mermaid
graph LR
    subgraph "Output Receptors"
        UI[UI Display Receptor]
        FILE_OUT[File Writer Receptor]
        DB[Database Receptor]
        EMAIL[Email Receptor]
        SPEECH[Text-to-Speech Receptor]
        CHART[Chart Receptor]
        REPORT[Report Generator]
    end
    
    subgraph "Semantic Input"
        DATA[Processed Data]
        RESULTS[Analysis Results]
        EVENTS[System Events]
        CONTENT[Content Items]
    end
    
    subgraph "Output Destinations"
        SCREEN[User Interface]
        FILES[File System]
        DATABASE[Database]
        MAILBOX[Email System]
        SPEAKERS[Audio Output]
        GRAPHICS[Charts/Graphs]
        DOCUMENTS[Reports]
    end
    
    DATA --> UI
    RESULTS --> CHART
    EVENTS --> FILE_OUT
    CONTENT --> DB
    DATA --> EMAIL
    CONTENT --> SPEECH
    RESULTS --> REPORT
    
    UI --> SCREEN
    CHART --> GRAPHICS
    FILE_OUT --> FILES
    DB --> DATABASE
    EMAIL --> MAILBOX
    SPEECH --> SPEAKERS
    REPORT --> DOCUMENTS
```

### 4. UI Receptors

UI receptors provide windowed interfaces for user interaction.

```mermaid
graph TB
    subgraph "UI Receptor Architecture"
        WBR[WindowedBaseReceptor]
        FORM[Windows Form]
        CONTROLS[UI Controls]
        EVENTS[UI Events]
        BINDING[Data Binding]
    end
    
    subgraph "User Interactions"
        CLICK[Button Clicks]
        INPUT[Text Input]
        SELECT[Selections]
        DRAG[Drag & Drop]
    end
    
    subgraph "Data Display"
        LISTS[Data Lists]
        GRIDS[Data Grids]
        CHARTS[Charts]
        IMAGES[Images]
        TEXT[Text Display]
    end
    
    WBR --> FORM
    FORM --> CONTROLS
    CONTROLS --> EVENTS
    CONTROLS --> BINDING
    
    CLICK --> EVENTS
    INPUT --> EVENTS
    SELECT --> EVENTS
    DRAG --> EVENTS
    
    BINDING --> LISTS
    BINDING --> GRIDS
    BINDING --> CHARTS
    BINDING --> IMAGES
    BINDING --> TEXT
```

## Communication Patterns

### Subscription Patterns

```mermaid
graph TB
    subgraph "Subscription Types"
        SIMPLE[Simple Subscription]
        QUALIFIED[Qualified Subscription]
        FILTERED[Filtered Subscription]
        CONDITIONAL[Conditional Subscription]
    end
    
    subgraph "Qualifier Types"
        CONTENT[Content-Based]
        SOURCE[Source-Based]
        TIME[Time-Based]
        STATE[State-Based]
    end
    
    QUALIFIED --> CONTENT
    QUALIFIED --> SOURCE
    QUALIFIED --> TIME
    QUALIFIED --> STATE
    
    FILTERED --> QUALIFIED
    CONDITIONAL --> QUALIFIED
```

#### Subscription Examples

```csharp
// Simple subscription - receive all instances
Subscribe<RSSFeedItem>();

// Qualified subscription - filter by content
Subscribe<RSSFeedItem>(new ReceiveQualifier
{
    ContentFilter = item => item.RSSFeedName.Text.Value.Contains("Technology")
});

// State-based subscription - conditional on receptor state
Subscribe<TimerEvent>(new ReceiveQualifier
{
    Condition = () => IsProcessingEnabled && !IsBusy
});
```

### Emission Patterns

```mermaid
sequenceDiagram
    participant R as Source Receptor
    participant M as Membrane
    participant R1 as Receptor 1
    participant R2 as Receptor 2
    participant R3 as Receptor 3
    
    Note over R,M: Broadcast Pattern
    R->>M: Emit(SemanticData)
    M->>R1: Route to Subscriber 1
    M->>R2: Route to Subscriber 2
    M->>R3: Route to Subscriber 3
    
    Note over R,M: Selective Pattern
    R->>M: Emit(FilteredData)
    M->>M: Apply Filters
    M->>R2: Route to Qualified Subscriber
    
    Note over R,M: Response Pattern
    R1->>M: Emit(Request)
    M->>R: Route Request
    R->>M: Emit(Response)
    M->>R1: Route Response
```

## Lifecycle Management

### Receptor States

```mermaid
stateDiagram-v2
    [*] --> Created
    Created --> Initializing : Initialize()
    Initializing --> Ready : Success
    Initializing --> Error : Failure
    
    Ready --> Processing : Receive Message
    Processing --> Ready : Complete
    Processing --> Error : Exception
    
    Ready --> Terminating : Terminate()
    Processing --> Terminating : Terminate()
    Terminating --> Terminated : Success
    Error --> Terminated : Terminate()
    
    Terminated --> [*]
```

### Lifecycle Events

```csharp
public interface IReceptorLifecycle
{
    event EventHandler<ReceptorEventArgs> Initializing;
    event EventHandler<ReceptorEventArgs> Initialized;
    event EventHandler<ReceptorEventArgs> Processing;
    event EventHandler<ReceptorEventArgs> Processed;
    event EventHandler<ReceptorEventArgs> Terminating;
    event EventHandler<ReceptorEventArgs> Terminated;
    event EventHandler<ReceptorErrorEventArgs> Error;
}
```

## Implementation Patterns

### Template Method Pattern

```csharp
public abstract class DataProcessorReceptor : BaseReceptor
{
    protected override void ProcessMessage(ICarrier carrier)
    {
        try
        {
            // Template method pattern
            var data = ExtractData(carrier);
            var validated = ValidateData(data);
            var processed = ProcessData(validated);
            var result = FormatResult(processed);
            EmitResult(result);
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }
    
    protected abstract object ExtractData(ICarrier carrier);
    protected virtual bool ValidateData(object data) => true;
    protected abstract object ProcessData(object data);
    protected virtual object FormatResult(object processed) => processed;
    protected abstract void EmitResult(object result);
    
    protected virtual void HandleError(Exception ex)
    {
        Emit(new ErrorInfo { Message = ex.Message, Source = Name });
    }
}
```

### Observer Pattern Integration

```mermaid
graph LR
    subgraph "Observer Pattern in HOPE"
        SUBJECT[Data Source]
        OBSERVERS[Multiple Receptors]
        MEMBRANE[Membrane as Mediator]
    end
    
    subgraph "Traditional Observer"
        TRAD_SUBJ[Subject]
        TRAD_OBS[Observers]
    end
    
    SUBJECT -->|publishes| MEMBRANE
    MEMBRANE -->|notifies| OBSERVERS
    
    TRAD_SUBJ -.->|direct notification| TRAD_OBS
    
    SUBJECT -.->|semantic decoupling| OBSERVERS
```

### State Machine Pattern

```csharp
public class StatefulReceptor : BaseReceptor
{
    private enum ProcessingState
    {
        Idle,
        Collecting,
        Processing,
        Outputting
    }
    
    private ProcessingState currentState = ProcessingState.Idle;
    private List<object> collectedData = new List<object>();
    
    protected override void ProcessMessage(ICarrier carrier)
    {
        switch (currentState)
        {
            case ProcessingState.Idle:
                HandleIdleState(carrier);
                break;
            case ProcessingState.Collecting:
                HandleCollectingState(carrier);
                break;
            case ProcessingState.Processing:
                HandleProcessingState(carrier);
                break;
            case ProcessingState.Outputting:
                HandleOutputtingState(carrier);
                break;
        }
    }
    
    private void TransitionTo(ProcessingState newState)
    {
        currentState = newState;
        Emit(new StateTransition { From = currentState, To = newState });
    }
}
```

## Best Practices

### Design Principles

```mermaid
graph TB
    subgraph "Receptor Design Principles"
        SRP[Single Responsibility]
        LOOSE[Loose Coupling]
        HIGH[High Cohesion]
        STATELESS[Prefer Stateless]
        ERROR[Error Handling]
        TEST[Testability]
    end
    
    subgraph "Implementation Guidelines"
        INTERFACE[Clear Interfaces]
        SEMANTIC[Semantic Clarity]
        CONFIG[Configuration]
        LOGGING[Logging]
        PERF[Performance]
        SECURITY[Security]
    end
    
    SRP --> INTERFACE
    LOOSE --> SEMANTIC
    HIGH --> CONFIG
    STATELESS --> LOGGING
    ERROR --> PERF
    TEST --> SECURITY
```

### 1. Single Responsibility
- Each receptor should have one clear purpose
- Avoid combining unrelated functionality

### 2. Semantic Clarity
- Use meaningful semantic type names
- Design types that reflect business concepts

### 3. Error Handling
- Always handle exceptions gracefully
- Emit error semantic types for downstream processing
- Implement proper logging

### 4. State Management
- Prefer stateless receptors when possible
- If state is needed, make it explicit and manageable
- Consider state persistence for critical receptors

### 5. Configuration
- Make receptors configurable through semantic types
- Support runtime reconfiguration when appropriate

### Example: Well-Designed Receptor

```csharp
public class ImageProcessorReceptor : BaseReceptor
{
    private ImageProcessingConfig config;
    
    public override void Initialize()
    {
        Subscribe<ImageFile>();
        Subscribe<ImageProcessingConfig>();
        
        // Default configuration
        config = new ImageProcessingConfig
        {
            MaxWidth = 800,
            MaxHeight = 600,
            Quality = 85
        };
    }
    
    protected override void ProcessMessage(ICarrier carrier)
    {
        try
        {
            switch (carrier.Message)
            {
                case ImageProcessingConfig newConfig:
                    config = newConfig;
                    Emit(new ConfigurationUpdated { Receptor = Name });
                    break;
                    
                case ImageFile imageFile:
                    ProcessImage(imageFile);
                    break;
            }
        }
        catch (Exception ex)
        {
            Emit(new ProcessingError
            {
                Source = Name,
                Message = ex.Message,
                Data = carrier.Message
            });
        }
    }
    
    private void ProcessImage(ImageFile imageFile)
    {
        // Single responsibility: image processing only
        var processedImage = ResizeImage(imageFile, config);
        
        Emit(new ProcessedImage
        {
            OriginalFile = imageFile,
            ProcessedData = processedImage,
            ProcessedAt = DateTime.Now,
            ProcessorConfig = config
        });
        
        // Emit operation completed event
        Emit(new OperationCompleted
        {
            Operation = "ImageProcessing",
            Success = true,
            Duration = TimeSpan.FromMilliseconds(100)
        });
    }
}
```

## Related Documentation

- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Overall system architecture
- **[Semantic-Type-System.md](Semantic-Type-System.md)** - Understanding semantic types used by receptors
- **[Data-Flow.md](Data-Flow.md)** - How data flows between receptors
- **[Examples.md](Examples.md)** - Practical receptor implementation examples