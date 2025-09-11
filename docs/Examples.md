# Practical Examples and Use Cases

## Table of Contents
- [Overview](#overview)
- [Hello World Example](#hello-world-example)
- [RSS Feed Reader Application](#rss-feed-reader-application)
- [APOD (Astronomy Picture of the Day) Application](#apod-astronomy-picture-of-the-day-application)
- [Weather Information System](#weather-information-system)
- [Hunt the Wumpus Game](#hunt-the-wumpus-game)
- [Natural Language Processing Pipeline](#natural-language-processing-pipeline)
- [Building Custom Applications](#building-custom-applications)
- [Best Practices from Examples](#best-practices-from-examples)

## Overview

This document provides practical examples of HOPE applications, from simple Hello World scenarios to complex multi-receptor systems. Each example demonstrates key architectural patterns and shows how semantic types and receptors work together to create emergent functionality.

## Hello World Example

The Hello World example demonstrates the basic receptor communication pattern.

### Application Architecture

```mermaid
graph LR
    subgraph "Hello World Application"
        TIMER[Timer Receptor]
        HELLO[Hello World Receptor]
        DISPLAY[Text Display Receptor]
        SPEECH[Text-to-Speech Receptor]
    end
    
    TIMER -->|TimerEvent| HELLO
    HELLO -->|HelloWorldMessage| DISPLAY
    HELLO -->|HelloWorldMessage| SPEECH
```

### Semantic Types

```xml
<!-- Timer Event -->
<SemanticType Name="TimerEvent">
  <SemanticElement Name="EventTime">
    <NativeType Name="Value" Type="DateTime" />
  </SemanticElement>
  <SemanticElement Name="Interval">
    <NativeType Name="Value" Type="TimeSpan" />
  </SemanticElement>
</SemanticType>

<!-- Hello World Message -->
<SemanticType Name="HelloWorldMessage">
  <SemanticElement Name="Message">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="Timestamp">
    <NativeType Name="Value" Type="DateTime" />
  </SemanticElement>
</SemanticType>
```

### Receptor Implementation

```csharp
public class HelloWorldReceptor : BaseReceptor
{
    private int messageCount = 0;
    
    public override void Initialize()
    {
        Subscribe<TimerEvent>();
    }
    
    protected override void ProcessMessage(ICarrier carrier)
    {
        if (carrier.Message is TimerEvent timerEvent)
        {
            messageCount++;
            
            var message = new HelloWorldMessage();
            message.Message.Text.Value = $"Hello World #{messageCount} at {DateTime.Now}";
            message.Timestamp.Value = DateTime.Now;
            
            Emit(message);
        }
    }
}

public class TextDisplayReceptor : WindowedBaseReceptor
{
    private ListBox messageList;
    
    public override void Initialize()
    {
        Subscribe<HelloWorldMessage>();
        CreateForm();
    }
    
    protected override void CreateForm()
    {
        Form = new Form { Text = "Hello World Display", Size = new Size(400, 300) };
        messageList = new ListBox { Dock = DockStyle.Fill };
        Form.Controls.Add(messageList);
        ShowForm();
    }
    
    protected override void ProcessMessage(ICarrier carrier)
    {
        if (carrier.Message is HelloWorldMessage message)
        {
            if (Form.InvokeRequired)
            {
                Form.Invoke(new Action(() => AddMessage(message)));
            }
            else
            {
                AddMessage(message);
            }
        }
    }
    
    private void AddMessage(HelloWorldMessage message)
    {
        messageList.Items.Add(message.Message.Text.Value);
        messageList.SelectedIndex = messageList.Items.Count - 1;
    }
}
```

### Data Flow Sequence

```mermaid
sequenceDiagram
    participant T as Timer Receptor
    participant M as Membrane
    participant H as Hello World Receptor
    participant D as Display Receptor
    participant S as Speech Receptor
    
    Note over T,S: Application Startup
    T->>M: Subscribe to nothing (timer driven)
    H->>M: Subscribe<TimerEvent>
    D->>M: Subscribe<HelloWorldMessage>
    S->>M: Subscribe<HelloWorldMessage>
    
    Note over T,S: Runtime Operation
    loop Every 5 seconds
        T->>M: Emit(TimerEvent)
        M->>H: Route TimerEvent
        H->>H: Generate Hello Message
        H->>M: Emit(HelloWorldMessage)
        M->>D: Route to Display
        M->>S: Route to Speech
        D->>D: Update UI
        S->>S: Speak Message
    end
```

## RSS Feed Reader Application

A more complex application that demonstrates data processing pipelines and multiple receptor types.

### Application Architecture

```mermaid
graph TB
    subgraph "RSS Feed Reader System"
        subgraph "Data Sources"
            TIMER[Timer Receptor]
            CONFIG[Configuration UI]
        end
        
        subgraph "Data Acquisition"
            RSS[RSS Reader Receptor]
            CACHE[Cache Manager]
        end
        
        subgraph "Data Processing"
            FILTER[Content Filter]
            DEDUP[Deduplication Receptor]
            ENRICH[Content Enricher]
        end
        
        subgraph "Data Storage"
            DB[Database Receptor]
            PERSIST[Persistence Manager]
        end
        
        subgraph "User Interface"
            FEED_LIST[Feed List Viewer]
            ITEM_DETAIL[Item Detail Viewer]
            SEARCH[Search Interface]
        end
        
        subgraph "External Services"
            FEEDS[RSS Feeds]
            DATABASE[SQL Database]
        end
    end
    
    TIMER -->|FeedRefreshEvent| RSS
    CONFIG -->|FeedConfiguration| RSS
    RSS -->|RSSFeedItem| FILTER
    RSS -->|RSSFeedItem| CACHE
    
    FILTER -->|FilteredContent| DEDUP
    DEDUP -->|UniqueContent| ENRICH
    ENRICH -->|EnrichedContent| DB
    
    DB -->|StorageEvent| PERSIST
    PERSIST --> DATABASE
    
    CACHE -->|CachedItems| FEED_LIST
    DB -->|QueryResults| ITEM_DETAIL
    DB -->|SearchResults| SEARCH
    
    RSS --> FEEDS
```

### Key Semantic Types

```xml
<!-- RSS Feed Configuration -->
<SemanticType Name="RSSFeedConfiguration">
  <SemanticElement Name="FeedUrl">
    <SemanticElement Name="Url">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="RefreshInterval">
    <NativeType Name="Value" Type="TimeSpan" />
  </SemanticElement>
  <SemanticElement Name="FilterKeywords">
    <SemanticElement Name="TextList">
      <NativeType Name="Values" Type="List&lt;string&gt;" />
    </SemanticElement>
  </SemanticElement>
</SemanticType>

<!-- RSS Feed Item -->
<SemanticType Name="RSSFeedItem">
  <SemanticElement Name="RSSFeedName" UniqueKey="true" Normalized="true">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="RSSFeedUrl" UniqueKey="true" Normalized="true">
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

<!-- Filtered Content -->
<SemanticType Name="FilteredContent">
  <SemanticElement Name="OriginalItem">
    <SemanticElement Name="RSSFeedItem" />
  </SemanticElement>
  <SemanticElement Name="FilterReason">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="FilteredAt">
    <NativeType Name="Value" Type="DateTime" />
  </SemanticElement>
</SemanticType>
```

### RSS Reader Receptor Implementation

```csharp
public class RSSReaderReceptor : BaseReceptor
{
    private Timer refreshTimer;
    private List<string> feedUrls = new List<string>();
    private TimeSpan refreshInterval = TimeSpan.FromMinutes(15);
    
    public override void Initialize()
    {
        Subscribe<RSSFeedConfiguration>();
        Subscribe<FeedRefreshEvent>();
        
        // Start with default refresh timer
        StartRefreshTimer();
    }
    
    protected override void ProcessMessage(ICarrier carrier)
    {
        switch (carrier.Message)
        {
            case RSSFeedConfiguration config:
                HandleConfiguration(config);
                break;
                
            case FeedRefreshEvent refreshEvent:
                RefreshFeeds();
                break;
        }
    }
    
    private void HandleConfiguration(RSSFeedConfiguration config)
    {
        feedUrls.Add(config.FeedUrl.Url.Value);
        refreshInterval = config.RefreshInterval.Value;
        
        // Restart timer with new interval
        StopRefreshTimer();
        StartRefreshTimer();
        
        // Immediate refresh for new feed
        RefreshSingleFeed(config.FeedUrl.Url.Value);
    }
    
    private void RefreshFeeds()
    {
        foreach (var feedUrl in feedUrls)
        {
            RefreshSingleFeed(feedUrl);
        }
    }
    
    private void RefreshSingleFeed(string feedUrl)
    {
        try
        {
            var rssDocument = LoadRSSFeed(feedUrl);
            var feedName = ExtractFeedName(rssDocument);
            
            foreach (var item in rssDocument.Items)
            {
                var rssItem = new RSSFeedItem();
                rssItem.RSSFeedName.Text.Value = feedName;
                rssItem.RSSFeedUrl.Url.Value = item.Link;
                rssItem.RSSFeedTitle.Text.Value = item.Title;
                rssItem.RSSFeedDescription.Text.Value = item.Description;
                rssItem.RSSFeedPubDate.Value = item.PublishDate;
                
                Emit(rssItem);
            }
            
            Emit(new FeedRefreshCompleted
            {
                FeedUrl = feedUrl,
                ItemCount = rssDocument.Items.Count,
                RefreshedAt = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            Emit(new FeedRefreshError
            {
                FeedUrl = feedUrl,
                ErrorMessage = ex.Message,
                ErrorTime = DateTime.Now
            });
        }
    }
}
```

## APOD (Astronomy Picture of the Day) Application

This example demonstrates image processing, web scraping, and UI integration.

### Application Components

```mermaid
graph TB
    subgraph "APOD Application"
        subgraph "Data Acquisition"
            TIMER[Daily Timer]
            SCRAPER[APOD Scraper]
            DOWNLOADER[Image Downloader]
        end
        
        subgraph "Image Processing"
            THUMB[Thumbnail Creator]
            META[Metadata Extractor]
            VALIDATOR[Image Validator]
        end
        
        subgraph "Storage"
            IMG_STORE[Image Storage]
            META_DB[Metadata Database]
            CACHE[Image Cache]
        end
        
        subgraph "User Interface"
            GALLERY[Image Gallery]
            VIEWER[Full Image Viewer]
            INFO[Information Panel]
            SLIDESHOW[Slideshow Mode]
        end
        
        subgraph "Features"
            SEARCH[Image Search]
            FAVORITE[Favorites Manager]
            EXPORT[Image Export]
            WALLPAPER[Wallpaper Setter]
        end
    end
    
    TIMER -->|DailyTrigger| SCRAPER
    SCRAPER -->|APODInfo| DOWNLOADER
    DOWNLOADER -->|ImageFile| THUMB
    DOWNLOADER -->|ImageFile| META
    DOWNLOADER -->|ImageFile| VALIDATOR
    
    THUMB -->|ThumbnailImage| IMG_STORE
    META -->|ImageMetadata| META_DB
    VALIDATOR -->|ValidatedImage| CACHE
    
    IMG_STORE -->|StoredImages| GALLERY
    CACHE -->|CachedImages| VIEWER
    META_DB -->|Metadata| INFO
    
    GALLERY -->|SelectedImage| VIEWER
    VIEWER -->|ImageSelection| INFO
    INFO -->|SearchQuery| SEARCH
    GALLERY -->|FavoriteAction| FAVORITE
    VIEWER -->|ExportRequest| EXPORT
    VIEWER -->|WallpaperRequest| WALLPAPER
```

### APOD Semantic Types

```xml
<SemanticType Name="APODInfo">
  <SemanticElement Name="Title">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="Explanation">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="ImageUrl">
    <SemanticElement Name="Url">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="HighDefUrl">
    <SemanticElement Name="Url">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="Date">
    <NativeType Name="Value" Type="DateTime" />
  </SemanticElement>
  <SemanticElement Name="MediaType">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="Copyright">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
</SemanticType>

<SemanticType Name="ImageFile">
  <SemanticElement Name="Filename">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="ImageData">
    <NativeType Name="Value" Type="byte[]" />
  </SemanticElement>
  <SemanticElement Name="Size">
    <NativeType Name="Value" Type="long" />
  </SemanticElement>
  <SemanticElement Name="Format">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="Source">
    <SemanticElement Name="Url">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
</SemanticType>

<SemanticType Name="ThumbnailImage">
  <SemanticElement Name="OriginalImage">
    <SemanticElement Name="ImageFile" />
  </SemanticElement>
  <SemanticElement Name="ThumbnailData">
    <NativeType Name="Value" Type="byte[]" />
  </SemanticElement>
  <SemanticElement Name="Width">
    <NativeType Name="Value" Type="int" />
  </SemanticElement>
  <SemanticElement Name="Height">
    <NativeType Name="Value" Type="int" />
  </SemanticElement>
</SemanticType>
```

### APOD Processing Flow

```mermaid
sequenceDiagram
    participant T as Timer
    participant S as APOD Scraper
    participant D as Image Downloader
    participant TC as Thumbnail Creator
    participant IS as Image Storage
    participant G as Gallery UI
    participant V as Image Viewer
    
    Note over T,V: Daily APOD Processing
    T->>S: DailyTrigger
    S->>S: Scrape APOD Website
    S->>D: APODInfo
    
    D->>D: Download Image
    D->>TC: ImageFile
    D->>IS: ImageFile
    
    TC->>TC: Create Thumbnail
    TC->>IS: ThumbnailImage
    
    IS->>IS: Store Images
    IS->>G: StoredImage
    
    Note over G,V: User Interaction
    G->>V: UserSelectedImage
    V->>V: Display Full Image
    V->>V: Show Image Details
```

## Weather Information System

This example demonstrates external service integration and data aggregation.

### Weather System Architecture

```mermaid
graph TB
    subgraph "Weather Information System"
        subgraph "Data Sources"
            WS1[Weather Service 1]
            WS2[Weather Service 2]
            WS3[Weather Service 3]
            LOC[Location Service]
        end
        
        subgraph "Data Collection"
            WEATHER[Weather Receptor]
            GEO[Geo Location Receptor]
            TIMER[Update Timer]
        end
        
        subgraph "Data Processing"
            AGGREGATOR[Weather Aggregator]
            CONVERTER[Unit Converter]
            VALIDATOR[Data Validator]
            FORECASTER[Forecast Processor]
        end
        
        subgraph "Data Storage"
            CACHE[Weather Cache]
            HISTORY[Historical Data]
            CONFIG[User Preferences]
        end
        
        subgraph "User Interface"
            CURRENT[Current Weather]
            FORECAST[Forecast Display]
            MAP[Weather Map]
            ALERTS[Alert System]
        end
        
        subgraph "Features"
            NOTIFICATION[Notifications]
            WIDGET[Desktop Widget]
            EXPORT[Data Export]
            COMPARISON[Location Comparison]
        end
    end
    
    WS1 --> WEATHER
    WS2 --> WEATHER
    WS3 --> WEATHER
    LOC --> GEO
    
    TIMER -->|UpdateTrigger| WEATHER
    WEATHER -->|WeatherData| AGGREGATOR
    GEO -->|LocationInfo| AGGREGATOR
    
    AGGREGATOR -->|ProcessedWeather| CONVERTER
    CONVERTER -->|ConvertedData| VALIDATOR
    VALIDATOR -->|ValidatedWeather| FORECASTER
    
    FORECASTER -->|WeatherInfo| CACHE
    FORECASTER -->|WeatherInfo| HISTORY
    CACHE -->|CachedWeather| CURRENT
    HISTORY -->|HistoricalWeather| FORECAST
    
    CURRENT -->|WeatherAlert| ALERTS
    ALERTS -->|AlertInfo| NOTIFICATION
    CACHE -->|WeatherData| WIDGET
    HISTORY -->|WeatherData| EXPORT
    CACHE -->|WeatherData| COMPARISON
```

## Hunt the Wumpus Game

This example demonstrates a stateful game implementation using receptors.

### Game Architecture

```mermaid
graph TB
    subgraph "Hunt the Wumpus Game"
        subgraph "Game State"
            CAVE[Cave Configuration]
            PLAYER[Player Receptor]
            WUMPUS[Wumpus Receptor]
            GAME[Game Controller]
        end
        
        subgraph "User Interface"
            MAP[Cave Map Display]
            INPUT[Player Input]
            STATUS[Status Display]
            INVENTORY[Inventory Display]
        end
        
        subgraph "Game Logic"
            MOVEMENT[Movement Processor]
            COMBAT[Combat System]
            HAZARDS[Hazard Manager]
            SCORING[Score Keeper]
        end
        
        subgraph "Game Events"
            MOVE[Move Events]
            ENCOUNTER[Encounter Events]
            VICTORY[Victory Events]
            DEFEAT[Defeat Events]
        end
    end
    
    INPUT -->|PlayerAction| MOVEMENT
    MOVEMENT -->|MoveCommand| PLAYER
    PLAYER -->|PlayerPosition| CAVE
    CAVE -->|CaveInfo| MAP
    
    PLAYER -->|PlayerPosition| HAZARDS
    HAZARDS -->|HazardEncounter| COMBAT
    COMBAT -->|CombatResult| GAME
    
    GAME -->|GameEvent| STATUS
    GAME -->|ScoreUpdate| SCORING
    SCORING -->|ScoreInfo| STATUS
    
    CAVE -->|WumpusPosition| WUMPUS
    WUMPUS -->|WumpusAction| COMBAT
    COMBAT -->|GameOutcome| VICTORY
    COMBAT -->|GameOutcome| DEFEAT
```

### Game Semantic Types

```xml
<SemanticType Name="PlayerPosition">
  <SemanticElement Name="RoomNumber">
    <NativeType Name="Value" Type="int" />
  </SemanticElement>
  <SemanticElement Name="ArrowCount">
    <NativeType Name="Value" Type="int" />
  </SemanticElement>
  <SemanticElement Name="IsAlive">
    <NativeType Name="Value" Type="bool" />
  </SemanticElement>
</SemanticType>

<SemanticType Name="CaveConfiguration">
  <SemanticElement Name="RoomConnections">
    <NativeType Name="Value" Type="Dictionary&lt;int, List&lt;int&gt;&gt;" />
  </SemanticElement>
  <SemanticElement Name="WumpusRoom">
    <NativeType Name="Value" Type="int" />
  </SemanticElement>
  <SemanticElement Name="PitRooms">
    <NativeType Name="Value" Type="List&lt;int&gt;" />
  </SemanticElement>
  <SemanticElement Name="BatRooms">
    <NativeType Name="Value" Type="List&lt;int&gt;" />
  </SemanticElement>
</SemanticType>

<SemanticType Name="PlayerAction">
  <SemanticElement Name="ActionType">
    <SemanticElement Name="Text">
      <NativeType Name="Value" Type="string" />
    </SemanticElement>
  </SemanticElement>
  <SemanticElement Name="TargetRoom">
    <NativeType Name="Value" Type="int" />
  </SemanticElement>
  <SemanticElement Name="ActionTime">
    <NativeType Name="Value" Type="DateTime" />
  </SemanticElement>
</SemanticType>
```

## Natural Language Processing Pipeline

This example shows a complex data processing pipeline with multiple analysis stages.

### NLP Pipeline Architecture

```mermaid
graph TB
    subgraph "NLP Processing Pipeline"
        subgraph "Input Sources"
            RSS[RSS Feeds]
            DOCS[Document Upload]
            SOCIAL[Social Media]
            EMAIL[Email Content]
        end
        
        subgraph "Text Preprocessing"
            EXTRACTOR[Text Extractor]
            CLEANER[Text Cleaner]
            TOKENIZER[Tokenizer]
            NORMALIZER[Text Normalizer]
        end
        
        subgraph "NLP Analysis"
            POS[POS Tagger]
            NER[Named Entity Recognition]
            SENTIMENT[Sentiment Analysis]
            TOPICS[Topic Modeling]
            KEYWORDS[Keyword Extraction]
        end
        
        subgraph "Knowledge Processing"
            ENTITIES[Entity Linking]
            RELATIONS[Relationship Extraction]
            CONCEPTS[Concept Mapping]
            SUMMARIZER[Text Summarization]
        end
        
        subgraph "Output Processing"
            INDEXER[Search Indexing]
            CLASSIFIER[Document Classification]
            RECOMMENDER[Content Recommendation]
            REPORTER[Analysis Reports]
        end
        
        subgraph "Data Storage"
            TEXT_DB[Text Database]
            KNOWLEDGE[Knowledge Base]
            ANALYTICS[Analytics Store]
        end
    end
    
    RSS --> EXTRACTOR
    DOCS --> EXTRACTOR
    SOCIAL --> EXTRACTOR
    EMAIL --> EXTRACTOR
    
    EXTRACTOR --> CLEANER
    CLEANER --> TOKENIZER
    TOKENIZER --> NORMALIZER
    
    NORMALIZER --> POS
    NORMALIZER --> NER
    NORMALIZER --> SENTIMENT
    NORMALIZER --> TOPICS
    NORMALIZER --> KEYWORDS
    
    NER --> ENTITIES
    POS --> RELATIONS
    TOPICS --> CONCEPTS
    KEYWORDS --> SUMMARIZER
    
    ENTITIES --> INDEXER
    RELATIONS --> CLASSIFIER
    CONCEPTS --> RECOMMENDER
    SUMMARIZER --> REPORTER
    
    INDEXER --> TEXT_DB
    CLASSIFIER --> KNOWLEDGE
    RECOMMENDER --> ANALYTICS
    REPORTER --> ANALYTICS
```

### NLP Data Flow

```mermaid
sequenceDiagram
    participant RSS as RSS Receptor
    participant EXT as Text Extractor
    participant CLEAN as Text Cleaner
    participant NER as NER Processor
    participant SENT as Sentiment Analyzer
    participant IDX as Search Indexer
    participant DB as Database
    
    RSS->>EXT: RSSFeedItem
    EXT->>CLEAN: ExtractedText
    CLEAN->>NER: CleanedText
    CLEAN->>SENT: CleanedText
    
    NER->>IDX: NamedEntities
    SENT->>IDX: SentimentInfo
    
    IDX->>DB: IndexedDocument
    DB->>DB: Store Processed Data
```

## Building Custom Applications

### Application Template

```csharp
public class CustomApplication
{
    private IMembrane membrane;
    private List<IReceptor> receptors = new List<IReceptor>();
    
    public void Initialize()
    {
        // Create membrane
        membrane = new Membrane();
        
        // Register semantic types
        RegisterSemanticTypes();
        
        // Create and register receptors
        CreateReceptors();
        
        // Start the application
        StartApplication();
    }
    
    private void RegisterSemanticTypes()
    {
        // Load semantic type definitions from XML
        var typeSystem = new SemanticTypeSystem();
        typeSystem.LoadFromFile("MyApplicationTypes.xml");
        membrane.RegisterTypeSystem(typeSystem);
    }
    
    private void CreateReceptors()
    {
        // Input receptors
        var inputReceptor = new MyInputReceptor();
        membrane.RegisterReceptor(inputReceptor);
        receptors.Add(inputReceptor);
        
        // Processing receptors
        var processingReceptor = new MyProcessingReceptor();
        membrane.RegisterReceptor(processingReceptor);
        receptors.Add(processingReceptor);
        
        // Output receptors
        var outputReceptor = new MyOutputReceptor();
        membrane.RegisterReceptor(outputReceptor);
        receptors.Add(outputReceptor);
        
        // UI receptors
        var uiReceptor = new MyUIReceptor();
        membrane.RegisterReceptor(uiReceptor);
        receptors.Add(uiReceptor);
    }
    
    private void StartApplication()
    {
        // Initialize all receptors
        foreach (var receptor in receptors)
        {
            receptor.Initialize();
        }
        
        // Start the membrane
        membrane.Start();
    }
    
    public void Shutdown()
    {
        // Terminate receptors
        foreach (var receptor in receptors)
        {
            receptor.Terminate();
        }
        
        // Stop the membrane
        membrane.Stop();
    }
}
```

### Custom Receptor Template

```csharp
public class MyCustomReceptor : BaseReceptor
{
    private MyConfiguration config;
    private MyInternalState state;
    
    public override void Initialize()
    {
        // Subscribe to semantic types this receptor processes
        Subscribe<MyInputType>();
        Subscribe<MyConfigurationType>();
        
        // Initialize internal state
        state = new MyInternalState();
        
        // Load default configuration
        LoadDefaultConfiguration();
    }
    
    protected override void ProcessMessage(ICarrier carrier)
    {
        try
        {
            switch (carrier.Message)
            {
                case MyInputType input:
                    ProcessInput(input);
                    break;
                    
                case MyConfigurationType configuration:
                    UpdateConfiguration(configuration);
                    break;
                    
                default:
                    // Handle unexpected message types
                    EmitError($"Unexpected message type: {carrier.Message.GetType().Name}");
                    break;
            }
        }
        catch (Exception ex)
        {
            EmitError($"Error processing message: {ex.Message}");
        }
    }
    
    private void ProcessInput(MyInputType input)
    {
        // Validate input
        if (!ValidateInput(input))
        {
            EmitError("Invalid input data");
            return;
        }
        
        // Process the input
        var result = PerformProcessing(input);
        
        // Update internal state
        UpdateState(result);
        
        // Emit result
        EmitResult(result);
    }
    
    private void UpdateConfiguration(MyConfigurationType configuration)
    {
        config = configuration;
        
        // Emit configuration update event
        Emit(new ConfigurationUpdated
        {
            ReceptorName = Name,
            UpdateTime = DateTime.Now
        });
    }
    
    private void EmitError(string message)
    {
        Emit(new ErrorInfo
        {
            Source = Name,
            ErrorMessage = message,
            ErrorTime = DateTime.Now
        });
    }
    
    private void EmitResult(MyResultType result)
    {
        Emit(result);
        
        // Also emit processing statistics
        Emit(new ProcessingStatistics
        {
            ReceptorName = Name,
            ProcessingTime = DateTime.Now,
            InputCount = state.InputCount,
            OutputCount = state.OutputCount
        });
    }
}
```

## Best Practices from Examples

### Design Patterns

1. **Separation of Concerns**: Each receptor has a single, well-defined responsibility
2. **Error Handling**: All receptors emit error semantic types for problems
3. **Configuration Management**: Use semantic types for configuration updates
4. **State Management**: Keep state minimal and well-encapsulated
5. **Resource Management**: Properly dispose of resources in receptor lifecycle

### Performance Considerations

1. **Asynchronous Processing**: Use async patterns for I/O-bound operations
2. **Batching**: Process multiple items together when possible
3. **Caching**: Cache frequently accessed data
4. **Resource Pooling**: Reuse expensive objects
5. **Memory Management**: Avoid memory leaks in long-running receptors

### Testing Strategies

1. **Unit Testing**: Test receptors in isolation with mock carriers
2. **Integration Testing**: Test receptor chains and data flows
3. **System Testing**: Test complete applications end-to-end
4. **Performance Testing**: Measure throughput and resource usage
5. **Error Testing**: Verify proper error handling and recovery

### Documentation Guidelines

1. **Semantic Type Documentation**: Document the meaning and usage of each type
2. **Receptor Documentation**: Explain purpose, inputs, outputs, and behavior
3. **Application Documentation**: Document overall architecture and data flows
4. **Configuration Documentation**: Document all configuration options
5. **Example Documentation**: Provide working examples and tutorials

## Related Documentation

- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Overall system architecture
- **[Semantic-Type-System.md](Semantic-Type-System.md)** - Understanding semantic types used in examples
- **[Receptor-Architecture.md](Receptor-Architecture.md)** - Receptor design patterns
- **[Data-Flow.md](Data-Flow.md)** - How data flows through example applications