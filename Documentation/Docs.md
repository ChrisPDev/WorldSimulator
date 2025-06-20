# 🌍 World Simulator Documentation

---

## 🧭 Overview
Brief description of the project, its goals, and how it works.

## 🏗️ Architecture
High-level explanation of how the components interact.

## 📂 Commands

### `RelayCommand.cs`

- **Purpose**: Implements the `ICommand` interface to allow binding UI actions to ViewModel logic in the MVVM pattern.
- **Key Members**:
  - `RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)`: Constructor that takes delegates for execution and condition checking.
  - `CanExecute(object parameter)`: Determines whether the command can execute.
  - `Execute(object parameter)`: Executes the command.
  - `CanExecuteChanged`: Event that triggers re-evaluation of `CanExecute`.
- **Usage**: Used in ViewModels to bind commands to UI elements like buttons.
- **Notes**: This is a standard implementation of a relay command. It supports parameterized commands and integrates with WPF's `CommandManager`.

## 📂 Config

### `SimulationConfig.cs`

- **Purpose**: Defines core constants that configure the simulation world’s dimensions and cell size.
- **Key Members**:
  - `MapWidth`, `MapHeight`: Dimensions of the world map in cells.
  - `ChunkWidth`, `ChunkHeight`: Dimensions of each chunk in cells.
  - `CellSize`: The pixel size of each cell on the canvas.
- **Usage**: Used throughout the simulation to ensure consistent sizing and layout.
- **Notes**: Consider externalizing these values to a config file if you want runtime configurability in the future.

## 📂 Converters

### `CellToCanvasXConverter.cs`

- **Purpose**: Converts a cell's X-coordinate (grid-based) into a canvas X-position (pixel-based) using the configured cell size.
- **Key Members**:
  - `Convert`: Multiplies the X-coordinate by `CellSize` to get the canvas position.
  - `ConvertBack`: Not implemented (one-way binding).
- **Usage**: Used in XAML bindings to position elements horizontally on the canvas.
- **Notes**: Uses `SimulationConfig.CellSize` for consistent scaling.

### `CellToCanvasYConverter.cs`

- **Purpose**: Converts a cell's Y-coordinate (grid-based) into a canvas Y-position (pixel-based).
- **Key Members**:
  - `Convert`: Multiplies the Y-coordinate by `CellSize` to get the canvas position.
  - `ConvertBack`: Not implemented (one-way binding).
- **Usage**: Used in XAML bindings to position elements vertically on the canvas.
- **Notes**: Mirrors the logic of `CellToCanvasXConverter`.

## 📂 Models > NatureBase

### `Nature.cs`

- **Purpose**: Represents a generic natural entity with lifecycle stages and growth behavior.
- **Key Members**:
  - `Name`, `Type`, `Age`, `Lifespan`, `Stage`: Core properties defining the entity.
  - `GrowthHistory`: Tracks historical growth stage transitions.
  - `Logger`: Optional logger for recording growth events.
  - `IncrementAge()`: Increases the entity's age by one unit.
  - `SetLifespan(min, max)`: Randomly sets lifespan within a range.
  - `AttemptGrowth()`: Attempts to transition to the next growth stage based on age and probability.
  - `GetGrowthChance(min, max)`: Calculates chance of growth based on age factor.
  - `GetGrowthStageThresholds()`: Returns age thresholds for each growth stage.
- **Growth Stages** (`GrowthStage` enum):
  - `Plant`, `Young`, `Grown`, `Aged`, `Old`, `Dead`, `None`, `Produced`
- **Usage**: Base class for all nature types. Can be extended to include produce behavior or specialized growth logic.
- **Notes**:
  - Growth is probabilistic and age-dependent.
  - Designed for extensibility via virtual methods.

### `ProduceCapableNature.cs`

- **Purpose**: Abstract base class for nature types that can generate and manage produce (e.g., fruits, fungi, nuts).
- **Inherits**: `Nature`
- **Key Members**:
  - `ProduceItems`: List of currently held produce items.
  - `SupportedProduceTypes`: List of produce types this entity can generate.
  - `GetProduceType()`: Returns a comma-separated list of supported produce types.
  - `GetProduceSummary()`: Returns a summary of current produce counts.
  - `AttemptProduceGrowth()`: Attempts to grow new produce based on stage and probability.
  - `AgeAndDecayProduce()`: Ages produce items and removes them if they exceed their decay age.
- **Usage**: Used as a base for specific nature types like trees or bushes that can bear produce.
- **Notes**:
  - Growth only occurs in `Grown`, `Aged`, or `Old` stages.
  - Produce growth is probabilistic (40% chance per type per tick).
  - Decayed produce is removed and logged.
  - ✅ Now uses `ProduceType` enum directly for type comparisons instead of string matching.


### `ProduceType.cs`

- **Purpose**: Defines the types of produce that a nature entity can generate.
- **Members**:
  - `None`: No produce.
  - `Blossom`: Flowering produce.
  - `Fruit`: Edible fruit.
  - `Fungi`: Fungal produce like mushrooms.
  - `Nut`: Hard-shelled produce.
- **Usage**: Used in `ProduceCapableNature` to determine what kinds of produce an entity can grow and manage.
- **Notes**: Can be extended in the future to support more produce types.

## 📂 Models > Produce

### `Produce.cs`

- **Purpose**: Base class for all produce types. Manages age and decay logic.
- **Key Members**:
  - `Type`: The type of produce (e.g., `ProduceType.Fruit`, `ProduceType.Nut`).
  - `Age`: Current age of the produce item.
  - `DecayAge`: Age at which the produce decays and is removed.
  - `SetDecayAge(min, max)`: Randomly sets a decay age within a range.
- **Usage**: Inherited by specific produce types like `Fruit`, `Nut`, etc.
- **Notes**:
  - Uses `ProduceType` enum for type safety.
  - Decay age is randomized per instance.

---

### `Blossom.cs`

- **Purpose**: Represents a blossom-type produce.
- **Inherits**: `Produce`
- **Decay Range**: 2–5
- **Usage**: Grown by nature entities that support `ProduceType.Blossom`.

---

### `Fruit.cs`

- **Purpose**: Represents a fruit-type produce.
- **Inherits**: `Produce`
- **Decay Range**: 3–8
- **Usage**: Grown by nature entities that support `ProduceType.Fruit`.

---

### `Fungi.cs`

- **Purpose**: Represents a fungi-type produce (e.g., mushrooms).
- **Inherits**: `Produce`
- **Decay Range**: 8–25
- **Usage**: Grown by nature entities that support `ProduceType.Fungi`.
- **Notes**: Now marked as `public` for accessibility across the project.

---

### `Nut.cs`

- **Purpose**: Represents a nut-type produce.
- **Inherits**: `Produce`
- **Decay Range**: 10–20
- **Usage**: Grown by nature entities that support `ProduceType.Nut`.

## 📂 Models > World

### `BaseTerrainType.cs`

- **Purpose**: Defines the fundamental terrain types that can exist on the planet's surface.
- **Members**:
  - `Water`: Represents any water-covered terrain (e.g., ocean, lake).
  - `Land`: Represents any land-covered terrain (e.g., soil, sand).
- **Usage**: Used in `Cell` to define the base terrain type of each grid cell.
- **Notes**: Designed to be extended with subtypes in the future (e.g., Saltwater, Gravel).

---

### `Cell.cs`

- **Purpose**: Represents a single cell in the simulation grid.
- **Key Members**:
  - `X`, `Y`: World coordinates of the cell.
  - `IsSelected`: Indicates whether the cell is selected in the UI.
  - `TerrainType`: The base terrain type of the cell (e.g., Water or Land).
  - `SubTerrainType`: The specific terrain subtype (e.g., Saltwater, Soil).
  - `BackgroundBrush`: The fill color of the cell, based on terrain type.
  - `BorderBrush`: Dynamically changes based on selection state.
  - `ToString()`: Returns a string representation of the cell's coordinates.
- **Usage**: Used in the UI to represent and interact with individual grid cells.
- **Notes**:
  - Implements `INotifyPropertyChanged` for WPF data binding.
  - Terrain is initialized to `Water` and `Saltwater` by default.
  - `BackgroundBrush` reflects terrain visually and updates when terrain changes.

---

### `Chunk.cs`

- **Purpose**: Represents a rectangular group of `Cell` objects.
- **Key Members**:
  - `ChunkX`, `ChunkY`: Chunk coordinates in the world.
  - `Cells`: 2D array of `Cell` objects within the chunk.
  - `GetCell(x, y)`: Safely retrieves a cell within the chunk using local coordinates.
- **Usage**: Used to divide the world into manageable sections.
- **Notes**:
  - Cells are initialized with correct world coordinates.
  - `GetCell` adds encapsulation and bounds checking.

---

### `Planet.cs`
- **Purpose**: Represents a static planet that contains the simulation's `WorldMap`.
- **Key Members**:
  - `Name`: The name of the planet.
  - `WorldMap`: The 2D grid-based world map associated with the planet.
  - `TotalCells`: A derived property that returns the total number of cells in the world (`MapWidth * MapHeight`).
- **Usage**: Used to encapsulate the world map and provide a planetary context for the simulation.
- **Notes**:
  - The planet is currently static (no orbit, tilt, or rotation).
  - Designed to be simple and extensible for future planetary features.

---

### `SubTerrainType.cs`

- **Purpose**: Defines more specific terrain types that fall under the base terrain categories.
- **Members**:
  - `Saltwater`, `Freshwater`: Subtypes of `Water`.
  - `Soil`, `Sand`, `Gravel`: Subtypes of `Land`.
  - `Unknown`: Fallback for undefined or transitional terrain.
- **Usage**: Used in `Cell` to provide more detailed terrain classification.
- **Notes**: Can be used for biome generation, resource placement, or visual styling.

---

### `WorldMap.cs`

- **Purpose**: Represents the entire simulation world, composed of multiple chunks.
- **Key Members**:
  - `MapWidth`, `MapHeight`: Dimensions of the world in cells.
  - `ChunkWidth`, `ChunkHeight`: Dimensions of each chunk (cached from config).
  - `Chunks`: 2D array of `Chunk` objects.
  - `GetChunk(x, y)`: Safely retrieves a chunk by its coordinates.
  - `GetCell(worldX, worldY)`: Retrieves a cell by world coordinates using chunk and local cell lookup.
- **Usage**: Central structure for managing and accessing the simulation grid.
- **Notes**:
  - `ChunkWidth` and `ChunkHeight` are cached for performance and clarity.
  - Uses `Chunk.GetCell()` for encapsulated cell access.

## 📂 NatureTypes

### `Bush.cs`

- **Purpose**: Represents a bush that can grow produce.
- **Inherits**: `ProduceCapableNature`
- **Lifespan**: 10–50
- **Notes**: Supports multiple produce types via constructor.

---

### `Cactus.cs`

- **Purpose**: Represents a cactus capable of producing items.
- **Inherits**: `ProduceCapableNature`
- **Lifespan**: 10–200
- **Notes**: Supports multiple produce types via constructor.

---

### `Fern.cs`

- **Purpose**: Represents a non-producing fern.
- **Inherits**: `Nature`
- **Lifespan**: 5–100

---

### `Flower.cs`

- **Purpose**: Represents a short-lived flower.
- **Inherits**: `Nature`
- **Lifespan**: 3–7

---

### `Grass.cs`

- **Purpose**: Represents a fast-growing grass.
- **Inherits**: `Nature`
- **Lifespan**: 1–20

---

### `Moss.cs`

- **Purpose**: Represents moss with a moderate lifespan.
- **Inherits**: `Nature`
- **Lifespan**: 10–100

---

### `Mycelium.cs`

- **Purpose**: Represents a fungi-producing mycelium.
- **Inherits**: `ProduceCapableNature`
- **Lifespan**: 10–1000
- **Notes**: Can support multiple produce types.

---

### `Tree.cs`

- **Purpose**: Represents a long-living tree capable of producing items.
- **Inherits**: `ProduceCapableNature`
- **Lifespan**: 270–330
- **Notes**: Supports multiple produce types via constructor.

---

### `Vine.cs`

- **Purpose**: Represents a climbing vine.
- **Inherits**: `Nature`
- **Lifespan**: 5–50

## 📂 Utils

### `NatureLogger.cs`

- **Purpose**: Provides structured logging for lifecycle events of nature entities.
- **Key Members**:
  - `LogPlanted(name, age)`: Logs when an entity is planted.
  - `LogGrowth(name, from, to, age)`: Logs a growth stage transition.
  - `LogProduced(name, produceType, age)`: Logs when produce is generated.
  - `LogDecayed(name, produceType, age)`: Logs when produce decays.
- **Usage**: Used by `Nature` and `ProduceCapableNature` to record and report simulation events.
- **Notes**:
  - Logs are both stored in a list and passed to a callback for real-time display or output.
  - Messages are formatted for readability and consistency.

## 📂 ViewModels

### `MainViewModel.cs`

- **Purpose**: Acts as the central ViewModel in the MVVM architecture. Manages the simulation world, nature elements, selection, logging, and now the planetary context.
- **Key Members**:
  - `WorldMap`: The simulation grid composed of chunks and cells.
  - `Planet`: Represents the planet that contains the simulation's `WorldMap`.
  - `Elements`: Observable collection of all nature entities in the simulation.
  - `GrowthLogMessages`: Logs growth and produce events for the selected entity.
  - `AllCells`: Flattened list of all cells in the world for UI binding.
  - `CreateTestDataCommand`: Command to populate the simulation with test data.
  - `CurrentSimYear`: Exposes the current simulation year from the `SimulationManager`.
  - `Selection`: Exposes the `SelectionManager` for UI interaction.
  - `CreateTestData()`: Populates the simulation with predefined nature entities and sets up logging.
  - `CreateNatureObservableCollection()`: Returns a predefined list of nature entities.
  - `OnSimulationTick()`: Updates selection and notifies UI of simulation year changes.
- **Usage**: Bound to the main window. Coordinates simulation state, user selection, and UI updates.
- **Notes**:
  - Uses `RelayCommand` for UI interaction.
  - Integrates with `SimulationManager` and `SelectionManager` for simulation logic.
  - Logging is routed through `NatureLogger` and filtered by selection.
  - The `Planet` property provides a high-level container for the world and can be extended with planetary features in the future.


---

### `SelectionManager.cs`

- **Purpose**: Manages selection state for nature entities and cells in the simulation.
- **Key Members**:
  - `SelectedIndex`: Index of the currently selected nature entity.
  - `SelectedNature`: The currently selected nature entity.
  - `SelectedCell`: The currently selected cell in the world.
  - `SelectedPlantName`, `SelectedPlantType`, `SelectedPlantProduce`, `SelectedPlantAge`, `SelectedPlantLifespan`, `SelectedPlantStage`: Exposed properties for UI binding.
  - `SelectedChunkX`, `SelectedChunkY`: Chunk coordinates of the selected cell.
  - `SelectedCellDisplay`, `SelectedChunkDisplay`: Formatted display strings for UI.
  - `SelectedCellBaseTerrain`: The base terrain type of the selected cell.
  - `SelectedCellSubTerrain`: The sub-terrain type of the selected cell.
  - `UpdateSelectedNature()`: Updates the selected nature entity and refreshes the growth log.
  - `UpdateGrowthLogMessages(Nature)`: Populates the log messages for the selected entity.
  - `NotifyNaturePropertiesChanged()`: Notifies the UI of changes to selected nature properties.
- **Usage**: Used by `MainViewModel` to manage and expose selection-related data to the UI.
- **Notes**:
  - Implements `INotifyPropertyChanged` for WPF data binding.
  - Designed to be reactive to selection changes in both nature and cell contexts.
  - Terrain info is exposed for display in the UI.


---

### `SimulationManager.cs`

- **Purpose**: Controls the simulation loop and manages the lifecycle of nature entities.
- **Key Members**:
  - `CurrentSimYear`: Tracks the number of simulation ticks (years).
  - `Tick()`: Advances the simulation by one tick. Ages entities, triggers growth and produce logic, and removes expired entities.
- **Usage**: Instantiated by `MainViewModel` to run the simulation loop.
- **Notes**:
  - Uses a `DispatcherTimer` to trigger simulation ticks at regular intervals.
  - Supports both `Nature` and `ProduceCapableNature` behaviors.
  - Automatically removes entities that exceed their lifespan or become invalid.
  
## 📂 Views

### `MainWindow.xaml`

- **Purpose**: Defines the main user interface layout for the simulation application. It includes controls for simulation interaction, plant selection, cell details, and a log viewer.
- **Key UI Regions**:
  - **Left Panel** (`Grid.Column="1"`):
    - `TestButton`: Button to trigger test data generation via `CreateTestDataCommand`.
    - `TestComboBoxCB`: Dropdown to select a nature entity from the simulation (`Elements`).
    - `Simulation Details`: Displays the current simulation year (`CurrentSimYear`).
    - `Plant Details`: Shows selected plant's name, type, produce, age, lifespan, and growth stage using `Selection` bindings.
    - `Selected Cell Info`: Displays the selected cell's map and chunk coordinates, as well as terrain type.
      - `Map X, Y`: Bound to `Selection.SelectedCellDisplay`.
      - `Chunk X, Y`: Bound to `Selection.SelectedChunkDisplay`.
      - `Base Terrain`: Bound to `Selection.SelectedCellBaseTerrain`.
      - `Sub Terrain`: Bound to `Selection.SelectedCellSubTerrain`.
  - **Center Panel** (`Grid.Column="2"`):
    - `MapViewControl`: Custom control (`MapView.xaml`) for rendering the simulation grid.
  - **Right Panel** (`Grid.Column="3"`):
    - `GrowthLogScrollViewer`: Scrollable list of log messages (`GrowthLogMessages`) related to the selected nature entity.
- **Styling**:
  - Uses centralized styles defined in `<Window.Resources>`:
    - `StandardTextStyle`: Applied to most `TextBlock` elements for consistent font and weight.
    - `TitleTextStyle`: Used for section headers like "Plant Details" and "Selected Cell Info".
    - `HorizontalTextCollectionStyle`: Used for horizontally aligned `StackPanel`s with label-value pairs.
- **Bindings**:
  - DataContext is expected to be `MainViewModel`.
  - Uses nested property bindings for `Selection.SelectedPlantName`, `Selection.SelectedCellDisplay`, `Selection.SelectedCellBaseTerrain`, etc.
- **Notes**:
  - Layout is structured using a `Grid` with three main columns and margin columns on each side.
  - Styles significantly reduce repetition and improve maintainability.
  - UI is designed for clarity and accessibility, with bold headers and centered alignment.

---

### `MainWindow.xaml.cs`

- **Purpose**: Code-behind for `MainWindow.xaml`. Initializes the main window and sets up the data context for the application.
- **Key Responsibilities**:
  - Instantiates `MainViewModel` and assigns it as the `DataContext` for data binding.
  - Passes the same ViewModel to the `MapViewControl` to ensure shared state and interaction.
- **Usage**: Automatically invoked when the application starts. Acts as the entry point for UI initialization.
- **Notes**:
  - Keeps logic minimal and delegates all behavior to the ViewModel.
  - Ensures that both the main window and the map view operate on the same simulation state.

---

### `MapView.xaml`

- **Purpose**: Custom user control that renders the simulation grid using a scrollable canvas layout. Each cell is represented as a colored square with interactive selection.
- **Key Components**:
  - `ScrollViewer`: Enables scrolling for large world maps.
  - `ItemsControl`: Binds to `AllCells` from the ViewModel and renders each cell as a `Border` on a `Canvas`.
  - `Canvas`: Used as the layout panel to position cells based on their world coordinates.
- **Bindings**:
  - `ItemsSource="{Binding AllCells}"`: Binds to the full list of cells in the simulation.
  - `Canvas.Left` and `Canvas.Top`: Use `CellToCanvasXConverter` and `CellToCanvasYConverter` to position each cell based on its grid coordinates.
  - `Background` and `BorderBrush`: Dynamically reflect the visual state of each cell (e.g., selection).
- **Events**:
  - `MouseLeftButtonDown="Border_MouseLeftButtonDown"`: Handles cell selection when a user clicks on a cell.
- **Resources**:
  - `CellToCanvasXConverter`, `CellToCanvasYConverter`: Convert cell grid coordinates to canvas pixel positions.
- **Notes**:
  - Uses `SimulationConfig.CellSize` to ensure consistent sizing across the simulation.
  - Designed for performance and scalability with large grids.
  - The control expects its `ViewModel` to be set externally (e.g., from `MainWindow.xaml.cs`).

---

### `MapView.xaml.cs`

- **Purpose**: Code-behind for `MapView.xaml`. Handles user interaction with the simulation grid, specifically cell selection.
- **Key Members**:
  - `ViewModel`: A reference to the `MainViewModel`, set when the `DataContext` changes.
- **Key Methods**:
  - `Border_MouseLeftButtonDown`: Handles mouse clicks on individual cells. Deselects all cells, selects the clicked one, and updates the `SelectedCell` in the `SelectionManager`.
- **Usage**: Automatically initialized when the `MapView` is loaded. Expects the `DataContext` to be set externally (e.g., from `MainWindow.xaml.cs`).
- **Notes**:
  - Uses event wiring in the constructor to capture `DataContextChanged` and assign the ViewModel.
  - Assumes that each `Border` in the UI is bound to a `Cell` object.
  - Ensures only one cell is selected at a time by clearing all other selections.
