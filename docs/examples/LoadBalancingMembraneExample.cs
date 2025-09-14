using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Clifton.Receptor;
using Clifton.Receptor.Interfaces;
using Clifton.SemanticTypeSystem.Interfaces;

namespace HOPE.Examples.MembraneComputing
{
    /// <summary>
    /// Example 3: Load Balancing with Dynamic Membrane Management
    /// This example demonstrates how to use membranes for dynamic load balancing
    /// and adaptive system configuration based on runtime conditions.
    /// </summary>
    public class LoadBalancingMembraneExample
    {
        private ISemanticTypeSystem sts;
        private IApplicationController appController;
        private Membrane rootMembrane;
        private Membrane loadBalancerMembrane;
        private List<Membrane> processorMembranes;
        private Membrane monitoringMembrane;
        private int currentProcessorCount;
        private bool isSystemRunning;

        // Simulated load metrics
        private Dictionary<Membrane, LoadMetrics> loadMetrics;

        public LoadBalancingMembraneExample(ISemanticTypeSystem semanticTypeSystem, IApplicationController applicationController)
        {
            sts = semanticTypeSystem;
            appController = applicationController;
            processorMembranes = new List<Membrane>();
            loadMetrics = new Dictionary<Membrane, LoadMetrics>();
            currentProcessorCount = 0;
            isSystemRunning = false;
        }

        /// <summary>
        /// Creates a load-balanced processing system with dynamic scaling capabilities.
        /// </summary>
        public void CreateLoadBalancedSystem(int initialProcessorCount = 3)
        {
            Console.WriteLine("Creating load-balanced membrane system...\n");

            // Create root membrane
            rootMembrane = new Membrane(sts, appController);
            rootMembrane.Name = "LoadBalancedSystem";

            // Create load balancer membrane
            CreateLoadBalancer();

            // Create initial processor membranes
            CreateInitialProcessors(initialProcessorCount);

            // Create monitoring membrane
            CreateMonitoringSystem();

            // Configure permeability for load balancing
            ConfigureLoadBalancingPermeability();

            // Set up dynamic scaling
            SetupDynamicScaling();

            Console.WriteLine($"Load-balanced system created with {initialProcessorCount} processors!");
            PrintSystemStatus();
        }

        /// <summary>
        /// Creates the load balancer membrane and its receptors.
        /// </summary>
        private void CreateLoadBalancer()
        {
            loadBalancerMembrane = rootMembrane.CreateInnerMembrane();
            loadBalancerMembrane.Name = "LoadBalancer";

            try
            {
                loadBalancerMembrane.RegisterReceptor("RequestDistributor", "RequestDistributorReceptor.dll");
                loadBalancerMembrane.RegisterReceptor("LoadAnalyzer", "LoadAnalyzerReceptor.dll");
                loadBalancerMembrane.RegisterReceptor("HealthChecker", "HealthCheckerReceptor.dll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Note: Could not load load balancer receptors (expected in this example): {ex.Message}");
            }

            Console.WriteLine("⚖️ Load balancer membrane created");
        }

        /// <summary>
        /// Creates the initial set of processor membranes.
        /// </summary>
        private void CreateInitialProcessors(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateProcessorMembrane(i);
            }

            currentProcessorCount = count;
            Console.WriteLine($"🔧 Created {count} initial processor membranes");
        }

        /// <summary>
        /// Creates a single processor membrane.
        /// </summary>
        private Membrane CreateProcessorMembrane(int id)
        {
            var processor = rootMembrane.CreateInnerMembrane();
            processor.Name = $"Processor_{id}";

            try
            {
                processor.RegisterReceptor($"Worker_{id}", "WorkerReceptor.dll");
                processor.RegisterReceptor($"TaskQueue_{id}", "TaskQueueReceptor.dll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Note: Could not load processor receptors for Processor_{id} (expected in this example): {ex.Message}");
            }

            // Configure processor permeability
            ConfigureProcessorPermeability(processor);

            // Initialize load metrics
            loadMetrics[processor] = new LoadMetrics
            {
                ProcessorId = id,
                CurrentLoad = 0.0,
                TasksProcessed = 0,
                AverageResponseTime = 100, // ms
                IsHealthy = true
            };

            processorMembranes.Add(processor);
            return processor;
        }

        /// <summary>
        /// Creates the monitoring system membrane.
        /// </summary>
        private void CreateMonitoringSystem()
        {
            monitoringMembrane = rootMembrane.CreateInnerMembrane();
            monitoringMembrane.Name = "MonitoringSystem";

            try
            {
                monitoringMembrane.RegisterReceptor("MetricsCollector", "MetricsCollectorReceptor.dll");
                monitoringMembrane.RegisterReceptor("AlertManager", "AlertManagerReceptor.dll");
                monitoringMembrane.RegisterReceptor("ScalingDecisionEngine", "ScalingEngineReceptor.dll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Note: Could not load monitoring receptors (expected in this example): {ex.Message}");
            }

            Console.WriteLine("📊 Monitoring system membrane created");
        }

        /// <summary>
        /// Configures permeability for the load balancing system.
        /// </summary>
        private void ConfigureLoadBalancingPermeability()
        {
            Console.WriteLine("Configuring load balancing permeability...");

            // Load balancer permeability
            ConfigureLoadBalancerPermeability();

            // Processor permeability (already done in CreateProcessorMembrane)
            
            // Monitoring permeability
            ConfigureMonitoringPermeability();

            // Update all permeability settings
            loadBalancerMembrane.UpdatePermeability();
            monitoringMembrane.UpdatePermeability();
            foreach (var processor in processorMembranes)
            {
                processor.UpdatePermeability();
            }

            Console.WriteLine("Load balancing permeability configured.");
        }

        /// <summary>
        /// Configures permeability for the load balancer membrane.
        /// </summary>
        private void ConfigureLoadBalancerPermeability()
        {
            // Allow incoming requests
            SetPermeability(loadBalancerMembrane, "IncomingRequest", PermeabilityDirection.In, true);
            SetPermeability(loadBalancerMembrane, "ProcessingRequest", PermeabilityDirection.In, true);

            // Allow distributing work to processors
            SetPermeability(loadBalancerMembrane, "WorkTask", PermeabilityDirection.Out, true);
            SetPermeability(loadBalancerMembrane, "LoadBalancingDecision", PermeabilityDirection.Out, true);

            // Allow receiving results and health checks
            SetPermeability(loadBalancerMembrane, "TaskResult", PermeabilityDirection.In, true);
            SetPermeability(loadBalancerMembrane, "HealthStatus", PermeabilityDirection.In, true);
            SetPermeability(loadBalancerMembrane, "LoadMetrics", PermeabilityDirection.In, true);

            // Allow sending final responses
            SetPermeability(loadBalancerMembrane, "ProcessingResponse", PermeabilityDirection.Out, true);
        }

        /// <summary>
        /// Configures permeability for a processor membrane.
        /// </summary>
        private void ConfigureProcessorPermeability(Membrane processor)
        {
            // Allow receiving work tasks
            SetPermeability(processor, "WorkTask", PermeabilityDirection.In, true);
            SetPermeability(processor, "LoadBalancingDecision", PermeabilityDirection.In, true);

            // Allow sending results and status
            SetPermeability(processor, "TaskResult", PermeabilityDirection.Out, true);
            SetPermeability(processor, "HealthStatus", PermeabilityDirection.Out, true);
            SetPermeability(processor, "LoadMetrics", PermeabilityDirection.Out, true);

            // Block internal processing details
            SetPermeability(processor, "InternalTaskQueue", PermeabilityDirection.Out, false);
            SetPermeability(processor, "WorkerInternalState", PermeabilityDirection.Out, false);
        }

        /// <summary>
        /// Configures permeability for the monitoring membrane.
        /// </summary>
        private void ConfigureMonitoringPermeability()
        {
            // Allow receiving metrics from all components
            SetPermeability(monitoringMembrane, "LoadMetrics", PermeabilityDirection.In, true);
            SetPermeability(monitoringMembrane, "HealthStatus", PermeabilityDirection.In, true);
            SetPermeability(monitoringMembrane, "PerformanceData", PermeabilityDirection.In, true);

            // Allow sending scaling decisions
            SetPermeability(monitoringMembrane, "ScaleUpDecision", PermeabilityDirection.Out, true);
            SetPermeability(monitoringMembrane, "ScaleDownDecision", PermeabilityDirection.Out, true);
            SetPermeability(monitoringMembrane, "AlertNotification", PermeabilityDirection.Out, true);

            // Allow monitoring data to leave for external systems
            SetPermeability(monitoringMembrane, "MonitoringReport", PermeabilityDirection.Out, true);
        }

        /// <summary>
        /// Sets up dynamic scaling based on load metrics.
        /// </summary>
        private void SetupDynamicScaling()
        {
            Console.WriteLine("Setting up dynamic scaling...");

            // Monitor system events for scaling decisions
            rootMembrane.NewCarrier += (sender, args) =>
            {
                HandleSystemEvent(args.Protocol.Protocol);
            };

            Console.WriteLine("Dynamic scaling configured.");
        }

        /// <summary>
        /// Handles system events for dynamic scaling.
        /// </summary>
        private void HandleSystemEvent(string protocolName)
        {
            switch (protocolName)
            {
                case "ScaleUpDecision":
                    ScaleUpSystem();
                    break;
                case "ScaleDownDecision":
                    ScaleDownSystem();
                    break;
                case "HighLoadAlert":
                    HandleHighLoadAlert();
                    break;
                case "ProcessorFailure":
                    HandleProcessorFailure();
                    break;
            }
        }

        /// <summary>
        /// Demonstrates the load balancing system in action.
        /// </summary>
        public void DemonstrateLoadBalancing()
        {
            Console.WriteLine("\n=== DEMONSTRATING LOAD BALANCING ===");

            isSystemRunning = true;

            // Start load simulation
            StartLoadSimulation();

            // Simulate various load conditions
            SimulateLoadConditions();

            isSystemRunning = false;
        }

        /// <summary>
        /// Starts a load simulation to show the system in action.
        /// </summary>
        private void StartLoadSimulation()
        {
            Console.WriteLine("\nStarting load simulation...");

            // Simulate processing load on each processor
            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"\nSimulation Step {i + 1}:");

                // Update load metrics for each processor
                foreach (var processor in processorMembranes)
                {
                    var metrics = loadMetrics[processor];
                    
                    // Simulate varying load
                    metrics.CurrentLoad = Math.Max(0, Math.Min(100, 
                        metrics.CurrentLoad + random.Next(-20, 30)));
                    
                    metrics.TasksProcessed += random.Next(5, 15);
                    metrics.AverageResponseTime = 50 + (int)(metrics.CurrentLoad * 2);
                    metrics.IsHealthy = metrics.CurrentLoad < 95;

                    PrintProcessorStatus(processor, metrics);
                }

                // Check if scaling is needed
                CheckScalingNeeds();

                Thread.Sleep(500); // Pause for demonstration
            }
        }

        /// <summary>
        /// Simulates various load conditions to trigger scaling.
        /// </summary>
        private void SimulateLoadConditions()
        {
            Console.WriteLine("\n=== LOAD CONDITION SIMULATION ===");

            // Simulate high load condition
            Console.WriteLine("\nSimulating HIGH LOAD condition...");
            SimulateHighLoad();

            Thread.Sleep(1000);

            // Simulate processor failure
            Console.WriteLine("\nSimulating PROCESSOR FAILURE...");
            SimulateProcessorFailure();

            Thread.Sleep(1000);

            // Simulate recovery
            Console.WriteLine("\nSimulating SYSTEM RECOVERY...");
            SimulateRecovery();
        }

        /// <summary>
        /// Simulates a high load condition.
        /// </summary>
        private void SimulateHighLoad()
        {
            foreach (var processor in processorMembranes)
            {
                var metrics = loadMetrics[processor];
                metrics.CurrentLoad = 85 + new Random().Next(0, 10);
                metrics.AverageResponseTime = 200 + new Random().Next(0, 100);
            }

            PrintSystemStatus();
            
            // This would trigger scale up in a real system
            Console.WriteLine("🔺 High load detected - would trigger scale up");
            ScaleUpSystem();
        }

        /// <summary>
        /// Simulates a processor failure.
        /// </summary>
        private void SimulateProcessorFailure()
        {
            if (processorMembranes.Count > 1)
            {
                var failedProcessor = processorMembranes.First();
                var metrics = loadMetrics[failedProcessor];
                metrics.IsHealthy = false;
                metrics.CurrentLoad = 0;

                Console.WriteLine($"💥 Processor {failedProcessor.Name} has failed");
                PrintProcessorStatus(failedProcessor, metrics);

                // Redistribute load to healthy processors
                RedistributeLoad(failedProcessor);
            }
        }

        /// <summary>
        /// Simulates system recovery.
        /// </summary>
        private void SimulateRecovery()
        {
            foreach (var processor in processorMembranes)
            {
                var metrics = loadMetrics[processor];
                metrics.CurrentLoad = 30 + new Random().Next(0, 20);
                metrics.AverageResponseTime = 80 + new Random().Next(0, 30);
                metrics.IsHealthy = true;
            }

            Console.WriteLine("✅ System recovered to normal operating conditions");
            PrintSystemStatus();
        }

        /// <summary>
        /// Scales up the system by adding new processor membranes.
        /// </summary>
        private void ScaleUpSystem()
        {
            if (currentProcessorCount >= 8) // Maximum limit
            {
                Console.WriteLine("⚠️ Cannot scale up: Maximum processor limit reached");
                return;
            }

            var newProcessor = CreateProcessorMembrane(currentProcessorCount);
            currentProcessorCount++;

            Console.WriteLine($"🔺 SCALED UP: Added {newProcessor.Name}");
            Console.WriteLine($"   Total processors: {currentProcessorCount}");

            // Update permeability for the new processor
            newProcessor.UpdatePermeability();

            PrintSystemStatus();
        }

        /// <summary>
        /// Scales down the system by removing processor membranes.
        /// </summary>
        private void ScaleDownSystem()
        {
            if (currentProcessorCount <= 1) // Minimum limit
            {
                Console.WriteLine("⚠️ Cannot scale down: Minimum processor limit reached");
                return;
            }

            // Find the processor with the lowest load
            var processorToRemove = FindLeastLoadedProcessor();
            
            if (processorToRemove != null)
            {
                Console.WriteLine($"🔻 SCALED DOWN: Removing {processorToRemove.Name}");
                
                // Redistribute its load
                RedistributeLoad(processorToRemove);
                
                // Remove the processor
                processorMembranes.Remove(processorToRemove);
                loadMetrics.Remove(processorToRemove);
                currentProcessorCount--;
                
                // Dissolve the membrane
                processorToRemove.Dissolve();
                
                Console.WriteLine($"   Total processors: {currentProcessorCount}");
                PrintSystemStatus();
            }
        }

        /// <summary>
        /// Handles high load alerts.
        /// </summary>
        private void HandleHighLoadAlert()
        {
            Console.WriteLine("🚨 HIGH LOAD ALERT received");
            
            // Check if scale up is needed
            var averageLoad = processorMembranes.Average(p => loadMetrics[p].CurrentLoad);
            
            if (averageLoad > 80)
            {
                Console.WriteLine($"   Average load: {averageLoad:F1}% - initiating scale up");
                ScaleUpSystem();
            }
            else
            {
                Console.WriteLine($"   Average load: {averageLoad:F1}% - within acceptable range");
            }
        }

        /// <summary>
        /// Handles processor failure events.
        /// </summary>
        private void HandleProcessorFailure()
        {
            Console.WriteLine("💥 PROCESSOR FAILURE event received");
            
            var failedProcessors = processorMembranes.Where(p => !loadMetrics[p].IsHealthy).ToList();
            
            foreach (var failed in failedProcessors)
            {
                Console.WriteLine($"   Handling failure of {failed.Name}");
                RedistributeLoad(failed);
            }
            
            // Consider scaling up to replace failed capacity
            if (failedProcessors.Count > 0)
            {
                ScaleUpSystem();
            }
        }

        /// <summary>
        /// Redistributes load from a failed or removed processor.
        /// </summary>
        private void RedistributeLoad(Membrane failedProcessor)
        {
            var healthyProcessors = processorMembranes
                .Where(p => p != failedProcessor && loadMetrics[p].IsHealthy)
                .ToList();

            if (healthyProcessors.Any())
            {
                Console.WriteLine($"   Redistributing load from {failedProcessor.Name} to {healthyProcessors.Count} healthy processors");
                
                var redistributedLoad = loadMetrics[failedProcessor].CurrentLoad / healthyProcessors.Count;
                
                foreach (var healthy in healthyProcessors)
                {
                    loadMetrics[healthy].CurrentLoad = Math.Min(100, 
                        loadMetrics[healthy].CurrentLoad + redistributedLoad);
                }
            }
        }

        /// <summary>
        /// Finds the processor with the least load.
        /// </summary>
        private Membrane FindLeastLoadedProcessor()
        {
            return processorMembranes
                .Where(p => loadMetrics[p].IsHealthy)
                .OrderBy(p => loadMetrics[p].CurrentLoad)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks if scaling is needed based on current metrics.
        /// </summary>
        private void CheckScalingNeeds()
        {
            var healthyProcessors = processorMembranes.Where(p => loadMetrics[p].IsHealthy).ToList();
            
            if (!healthyProcessors.Any()) return;

            var averageLoad = healthyProcessors.Average(p => loadMetrics[p].CurrentLoad);
            var maxLoad = healthyProcessors.Max(p => loadMetrics[p].CurrentLoad);

            // Scale up conditions
            if (averageLoad > 75 || maxLoad > 90)
            {
                Console.WriteLine($"📈 Scale up recommended: Avg={averageLoad:F1}%, Max={maxLoad:F1}%");
            }
            // Scale down conditions
            else if (averageLoad < 30 && healthyProcessors.Count > 2)
            {
                Console.WriteLine($"📉 Scale down possible: Avg={averageLoad:F1}%");
            }
        }

        /// <summary>
        /// Prints the status of a specific processor.
        /// </summary>
        private void PrintProcessorStatus(Membrane processor, LoadMetrics metrics)
        {
            string healthIcon = metrics.IsHealthy ? "✅" : "❌";
            string loadBar = CreateLoadBar(metrics.CurrentLoad);
            
            Console.WriteLine($"  {healthIcon} {processor.Name}: {loadBar} {metrics.CurrentLoad:F1}% " +
                            $"(Tasks: {metrics.TasksProcessed}, Resp: {metrics.AverageResponseTime}ms)");
        }

        /// <summary>
        /// Creates a visual load bar.
        /// </summary>
        private string CreateLoadBar(double load)
        {
            int bars = (int)(load / 10);
            string filled = new string('█', bars);
            string empty = new string('░', 10 - bars);
            return $"[{filled}{empty}]";
        }

        /// <summary>
        /// Prints the overall system status.
        /// </summary>
        public void PrintSystemStatus()
        {
            Console.WriteLine($"\n=== SYSTEM STATUS (Processors: {currentProcessorCount}) ===");

            foreach (var processor in processorMembranes)
            {
                if (loadMetrics.ContainsKey(processor))
                {
                    PrintProcessorStatus(processor, loadMetrics[processor]);
                }
            }

            if (loadMetrics.Any())
            {
                var healthyProcessors = processorMembranes.Where(p => loadMetrics[p].IsHealthy);
                var averageLoad = healthyProcessors.Any() ? 
                    healthyProcessors.Average(p => loadMetrics[p].CurrentLoad) : 0;
                var totalTasks = loadMetrics.Values.Sum(m => m.TasksProcessed);

                Console.WriteLine($"📊 System Overview:");
                Console.WriteLine($"   Average Load: {averageLoad:F1}%");
                Console.WriteLine($"   Total Tasks Processed: {totalTasks}");
                Console.WriteLine($"   Healthy Processors: {healthyProcessors.Count()}/{currentProcessorCount}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Helper method to set permeability.
        /// </summary>
        private void SetPermeability(Membrane membrane, string protocol, PermeabilityDirection direction, bool permeable)
        {
            var key = new PermeabilityKey { Protocol = protocol, Direction = direction };
            membrane.ProtocolPermeability[key] = new PermeabilityConfiguration { Permeable = permeable };
        }

        /// <summary>
        /// Cleans up the load balancing system.
        /// </summary>
        public void Cleanup()
        {
            if (rootMembrane != null)
            {
                isSystemRunning = false;
                rootMembrane.Reset();
                rootMembrane = null;
                processorMembranes.Clear();
                loadMetrics.Clear();
                Console.WriteLine("Load balancing system cleaned up.");
            }
        }
    }

    /// <summary>
    /// Represents load metrics for a processor membrane.
    /// </summary>
    public class LoadMetrics
    {
        public int ProcessorId { get; set; }
        public double CurrentLoad { get; set; } // 0-100%
        public int TasksProcessed { get; set; }
        public int AverageResponseTime { get; set; } // milliseconds
        public bool IsHealthy { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Helper class for running the load balancing membrane example.
    /// </summary>
    public class LoadBalancingRunner
    {
        public static void RunExample()
        {
            Console.WriteLine("=== HOPE Membrane Computing - Load Balancing Example ===\n");

            try
            {
                // Create mock semantic type system and application controller
                ISemanticTypeSystem sts = new SemanticTypeSystem();
                IApplicationController appController = new ApplicationController();

                var example = new LoadBalancingMembraneExample(sts, appController);

                // Create the load balanced system
                example.CreateLoadBalancedSystem(3);

                Console.WriteLine("\nPress any key to start load balancing demonstration...");
                Console.ReadKey();

                // Demonstrate load balancing
                example.DemonstrateLoadBalancing();

                Console.WriteLine("\nPress any key to see final system status...");
                Console.ReadKey();

                example.PrintSystemStatus();

                Console.WriteLine("\nPress any key to cleanup and exit...");
                Console.ReadKey();

                example.Cleanup();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running load balancing example: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            Console.WriteLine("\n=== Load Balancing Example Complete ===");
        }
    }
}