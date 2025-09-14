using System;
using System.Collections.Generic;
using Clifton.Receptor;
using Clifton.Receptor.Interfaces;
using Clifton.SemanticTypeSystem.Interfaces;

namespace HOPE.Examples.MembraneComputing
{
    /// <summary>
    /// Example 1: Basic Membrane Creation and Configuration
    /// This example demonstrates the fundamental concepts of membrane computing in HOPE.
    /// </summary>
    public class BasicMembraneExample
    {
        private ISemanticTypeSystem sts;
        private IApplicationController appController;
        private Membrane rootMembrane;

        public BasicMembraneExample(ISemanticTypeSystem semanticTypeSystem, IApplicationController applicationController)
        {
            sts = semanticTypeSystem;
            appController = applicationController;
        }

        /// <summary>
        /// Creates a basic membrane hierarchy with proper permeability configuration.
        /// </summary>
        public void CreateBasicMembraneSystem()
        {
            // Step 1: Create the root membrane
            rootMembrane = new Membrane(sts, appController);
            rootMembrane.Name = "RootMembrane";

            // Step 2: Create child membranes for different purposes
            var inputMembrane = rootMembrane.CreateInnerMembrane();
            inputMembrane.Name = "InputProcessor";

            var processingMembrane = rootMembrane.CreateInnerMembrane();
            processingMembrane.Name = "DataProcessor";

            var outputMembrane = rootMembrane.CreateInnerMembrane();
            outputMembrane.Name = "OutputHandler";

            // Step 3: Register receptors in appropriate membranes
            // Note: You would replace these with actual receptor assemblies
            try
            {
                inputMembrane.RegisterReceptor("FileReader", "FileReaderReceptor.dll");
                processingMembrane.RegisterReceptor("DataAnalyzer", "DataAnalyzerReceptor.dll");
                outputMembrane.RegisterReceptor("ReportGenerator", "ReportGeneratorReceptor.dll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Note: Could not load receptors (expected in this example): {ex.Message}");
            }

            // Step 4: Configure permeability
            ConfigureBasicPermeability(inputMembrane, processingMembrane, outputMembrane);

            // Step 5: Set up event handlers for monitoring
            SetupEventHandlers();

            Console.WriteLine("Basic membrane system created successfully!");
            PrintMembraneHierarchy(rootMembrane);
        }

        /// <summary>
        /// Configures permeability to allow proper data flow between membranes.
        /// </summary>
        private void ConfigureBasicPermeability(Membrane input, Membrane processing, Membrane output)
        {
            // Input membrane: allows raw data to leave
            var rawDataOutKey = new PermeabilityKey
            {
                Protocol = "RawData",
                Direction = PermeabilityDirection.Out
            };
            input.ProtocolPermeability[rawDataOutKey] = new PermeabilityConfiguration { Permeable = true };

            // Processing membrane: accepts raw data, emits processed data
            var rawDataInKey = new PermeabilityKey
            {
                Protocol = "RawData",
                Direction = PermeabilityDirection.In
            };
            processing.ProtocolPermeability[rawDataInKey] = new PermeabilityConfiguration { Permeable = true };

            var processedDataOutKey = new PermeabilityKey
            {
                Protocol = "ProcessedData",
                Direction = PermeabilityDirection.Out
            };
            processing.ProtocolPermeability[processedDataOutKey] = new PermeabilityConfiguration { Permeable = true };

            // Block internal processing data from leaving
            var internalProcessingKey = new PermeabilityKey
            {
                Protocol = "InternalProcessingState",
                Direction = PermeabilityDirection.Out
            };
            processing.ProtocolPermeability[internalProcessingKey] = new PermeabilityConfiguration { Permeable = false };

            // Output membrane: accepts processed data
            var processedDataInKey = new PermeabilityKey
            {
                Protocol = "ProcessedData",
                Direction = PermeabilityDirection.In
            };
            output.ProtocolPermeability[processedDataInKey] = new PermeabilityConfiguration { Permeable = true };

            // Update permeability after configuration
            input.UpdatePermeability();
            processing.UpdatePermeability();
            output.UpdatePermeability();

            Console.WriteLine("Permeability configured for data flow: Input → Processing → Output");
        }

        /// <summary>
        /// Sets up event handlers to monitor membrane activity.
        /// </summary>
        private void SetupEventHandlers()
        {
            rootMembrane.NewMembrane += (sender, args) => 
            {
                Console.WriteLine($"New membrane created: {args.Membrane.Name}");
            };

            rootMembrane.NewReceptor += (sender, args) => 
            {
                Console.WriteLine($"New receptor registered: {args.Receptor.Name}");
            };

            rootMembrane.NewCarrier += (sender, args) => 
            {
                Console.WriteLine($"New carrier created for protocol: {args.Protocol.Protocol}");
            };

            rootMembrane.ReceptorRemoved += (sender, args) => 
            {
                Console.WriteLine($"Receptor removed: {args.Receptor.Name}");
            };
        }

        /// <summary>
        /// Demonstrates dynamic permeability changes at runtime.
        /// </summary>
        public void DemonstrateRuntimeConfiguration()
        {
            if (rootMembrane == null)
            {
                Console.WriteLine("Please create the basic system first.");
                return;
            }

            var processingMembrane = FindMembraneByName("DataProcessor");
            if (processingMembrane == null)
            {
                Console.WriteLine("Processing membrane not found.");
                return;
            }

            Console.WriteLine("\nDemonstrating runtime permeability changes...");

            // Example: Temporarily allow debug information to leave the processing membrane
            var debugKey = new PermeabilityKey
            {
                Protocol = "DebugInfo",
                Direction = PermeabilityDirection.Out
            };

            // Enable debug output
            processingMembrane.ProtocolPermeability[debugKey] = new PermeabilityConfiguration { Permeable = true };
            processingMembrane.UpdatePermeability();
            Console.WriteLine("Debug output enabled for processing membrane");

            // Simulate some processing time
            System.Threading.Thread.Sleep(1000);

            // Disable debug output
            processingMembrane.ProtocolPermeability[debugKey] = new PermeabilityConfiguration { Permeable = false };
            processingMembrane.UpdatePermeability();
            Console.WriteLine("Debug output disabled for processing membrane");
        }

        /// <summary>
        /// Demonstrates moving receptors between membranes.
        /// </summary>
        public void DemonstrateReceptorMovement()
        {
            if (rootMembrane == null)
            {
                Console.WriteLine("Please create the basic system first.");
                return;
            }

            var inputMembrane = FindMembraneByName("InputProcessor");
            var processingMembrane = FindMembraneByName("DataProcessor");

            if (inputMembrane == null || processingMembrane == null)
            {
                Console.WriteLine("Required membranes not found.");
                return;
            }

            Console.WriteLine("\nDemonstrating receptor movement...");

            // Find a receptor to move (if any exist)
            if (inputMembrane.Receptors.Count > 0)
            {
                var receptorToMove = inputMembrane.Receptors[0];
                Console.WriteLine($"Moving receptor '{receptorToMove.Name}' from Input to Processing membrane");

                inputMembrane.MoveReceptorToMembrane(receptorToMove, processingMembrane);
                
                Console.WriteLine("Receptor moved successfully!");
                PrintMembraneHierarchy(rootMembrane);
            }
            else
            {
                Console.WriteLine("No receptors available to move in this example.");
            }
        }

        /// <summary>
        /// Demonstrates membrane dissolution.
        /// </summary>
        public void DemonstrateMembraneDissolve()
        {
            if (rootMembrane == null)
            {
                Console.WriteLine("Please create the basic system first.");
                return;
            }

            // Create a temporary membrane
            var tempMembrane = rootMembrane.CreateInnerMembrane();
            tempMembrane.Name = "TemporaryMembrane";

            Console.WriteLine("\nCreated temporary membrane:");
            PrintMembraneHierarchy(rootMembrane);

            // Dissolve the temporary membrane
            tempMembrane.Dissolve();

            Console.WriteLine("\nAfter dissolving temporary membrane:");
            PrintMembraneHierarchy(rootMembrane);
        }

        /// <summary>
        /// Utility method to find a membrane by name.
        /// </summary>
        private Membrane FindMembraneByName(string name)
        {
            if (rootMembrane.Name == name)
                return rootMembrane;

            foreach (var childMembrane in rootMembrane.Membranes)
            {
                if (childMembrane.Name == name)
                    return (Membrane)childMembrane;
            }

            return null;
        }

        /// <summary>
        /// Prints the membrane hierarchy for debugging.
        /// </summary>
        public void PrintMembraneHierarchy(Membrane membrane, int depth = 0)
        {
            string indent = new string(' ', depth * 2);
            Console.WriteLine($"{indent}📁 {membrane.Name} (Receptors: {membrane.Receptors.Count}, Children: {membrane.Membranes.Count})");

            // Print receptors in this membrane
            foreach (var receptor in membrane.Receptors)
            {
                Console.WriteLine($"{indent}  🔧 {receptor.Name}");
            }

            // Print permeability configuration
            if (membrane.ProtocolPermeability.Count > 0)
            {
                Console.WriteLine($"{indent}  Permeability:");
                foreach (var kvp in membrane.ProtocolPermeability)
                {
                    string arrow = kvp.Key.Direction == PermeabilityDirection.In ? "→" : "←";
                    string status = kvp.Value.Permeable ? "✅" : "❌";
                    Console.WriteLine($"{indent}    {status} {kvp.Key.Protocol} {arrow}");
                }
            }

            // Recursively print child membranes
            foreach (var child in membrane.Membranes)
            {
                PrintMembraneHierarchy((Membrane)child, depth + 1);
            }
        }

        /// <summary>
        /// Cleans up the membrane system.
        /// </summary>
        public void Cleanup()
        {
            if (rootMembrane != null)
            {
                rootMembrane.Reset();
                rootMembrane = null;
                Console.WriteLine("Membrane system cleaned up.");
            }
        }
    }

    /// <summary>
    /// Helper class for running the basic membrane example.
    /// </summary>
    public class BasicMembraneRunner
    {
        public static void RunExample()
        {
            Console.WriteLine("=== HOPE Membrane Computing - Basic Example ===\n");

            try
            {
                // Create mock semantic type system and application controller
                // In a real application, these would be properly initialized
                ISemanticTypeSystem sts = new SemanticTypeSystem();
                IApplicationController appController = new ApplicationController();

                var example = new BasicMembraneExample(sts, appController);

                // Run through the example steps
                example.CreateBasicMembraneSystem();

                Console.WriteLine("\nPress any key to continue to runtime configuration demo...");
                Console.ReadKey();

                example.DemonstrateRuntimeConfiguration();

                Console.WriteLine("\nPress any key to continue to receptor movement demo...");
                Console.ReadKey();

                example.DemonstrateReceptorMovement();

                Console.WriteLine("\nPress any key to continue to membrane dissolution demo...");
                Console.ReadKey();

                example.DemonstrateMembraneDissolve();

                Console.WriteLine("\nPress any key to cleanup and exit...");
                Console.ReadKey();

                example.Cleanup();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running example: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            Console.WriteLine("\n=== Example Complete ===");
        }
    }
}