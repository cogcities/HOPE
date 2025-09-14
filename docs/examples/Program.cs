using System;

namespace HOPE.Examples.MembraneComputing
{
    /// <summary>
    /// Main program to demonstrate all membrane computing examples.
    /// This provides a menu-driven interface to explore different aspects
    /// of membrane computing in HOPE.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║          HOPE Membrane Computing Examples            ║");
            Console.WriteLine("║                                                      ║");
            Console.WriteLine("║  A comprehensive demonstration of membrane computing ║");
            Console.WriteLine("║  patterns and implementations in the HOPE framework ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.WriteLine();

            bool exitRequested = false;

            while (!exitRequested)
            {
                DisplayMenu();
                var choice = Console.ReadLine()?.Trim();

                try
                {
                    switch (choice?.ToLower())
                    {
                        case "1":
                        case "basic":
                            RunBasicExample();
                            break;

                        case "2":
                        case "security":
                            RunSecurityExample();
                            break;

                        case "3":
                        case "load":
                        case "loadbalancing":
                            RunLoadBalancingExample();
                            break;

                        case "4":
                        case "all":
                            RunAllExamples();
                            break;

                        case "5":
                        case "help":
                            DisplayHelpInformation();
                            break;

                        case "6":
                        case "about":
                            DisplayAboutInformation();
                            break;

                        case "0":
                        case "exit":
                        case "quit":
                            exitRequested = true;
                            break;

                        default:
                            Console.WriteLine("❌ Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ An error occurred: {ex.Message}");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }

                if (!exitRequested)
                {
                    Console.WriteLine("\nPress any key to return to the main menu...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.WriteLine("Thank you for exploring HOPE Membrane Computing!");
            Console.WriteLine("For more information, visit: https://github.com/cliftonm/HOPE");
        }

        /// <summary>
        /// Displays the main menu options.
        /// </summary>
        private static void DisplayMenu()
        {
            Console.WriteLine("Please select an example to run:");
            Console.WriteLine();
            Console.WriteLine("🔧 1. Basic Membrane Operations");
            Console.WriteLine("   │ Learn fundamental membrane concepts:");
            Console.WriteLine("   │ • Creating membrane hierarchies");
            Console.WriteLine("   │ • Configuring permeability");
            Console.WriteLine("   │ • Moving receptors between membranes");
            Console.WriteLine("   │ • Dynamic membrane management");
            Console.WriteLine();
            Console.WriteLine("🔒 2. Security Boundaries");
            Console.WriteLine("   │ Explore security-focused membrane patterns:");
            Console.WriteLine("   │ • Multi-zone security architecture");
            Console.WriteLine("   │ • Protocol access controls");
            Console.WriteLine("   │ • Runtime security policy changes");
            Console.WriteLine("   │ • Security auditing and monitoring");
            Console.WriteLine();
            Console.WriteLine("⚖️  3. Load Balancing & Dynamic Scaling");
            Console.WriteLine("   │ Experience advanced membrane patterns:");
            Console.WriteLine("   │ • Dynamic processor membrane creation");
            Console.WriteLine("   │ • Automatic load balancing");
            Console.WriteLine("   │ • System scaling based on metrics");
            Console.WriteLine("   │ • Failure handling and recovery");
            Console.WriteLine();
            Console.WriteLine("🎯 4. Run All Examples");
            Console.WriteLine("   │ Execute all examples in sequence");
            Console.WriteLine();
            Console.WriteLine("❓ 5. Help & Documentation");
            Console.WriteLine("📖 6. About Membrane Computing");
            Console.WriteLine();
            Console.WriteLine("❌ 0. Exit");
            Console.WriteLine();
            Console.Write("Enter your choice (0-6): ");
        }

        /// <summary>
        /// Runs the basic membrane operations example.
        /// </summary>
        private static void RunBasicExample()
        {
            Console.Clear();
            Console.WriteLine("🔧 BASIC MEMBRANE OPERATIONS EXAMPLE");
            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.WriteLine("This example demonstrates:");
            Console.WriteLine("• Creating membrane hierarchies");
            Console.WriteLine("• Configuring permeability rules");
            Console.WriteLine("• Moving receptors between membranes");
            Console.WriteLine("• Dynamic membrane dissolution");
            Console.WriteLine();
            Console.WriteLine("Press any key to start the example...");
            Console.ReadKey();
            Console.Clear();

            BasicMembraneRunner.RunExample();
        }

        /// <summary>
        /// Runs the security boundaries example.
        /// </summary>
        private static void RunSecurityExample()
        {
            Console.Clear();
            Console.WriteLine("🔒 SECURITY BOUNDARIES EXAMPLE");
            Console.WriteLine("===============================");
            Console.WriteLine();
            Console.WriteLine("This example demonstrates:");
            Console.WriteLine("• Multi-zone security architecture");
            Console.WriteLine("• Strict permeability controls");
            Console.WriteLine("• Security boundary enforcement");
            Console.WriteLine("• Runtime security monitoring");
            Console.WriteLine("• Security audit capabilities");
            Console.WriteLine();
            Console.WriteLine("Press any key to start the example...");
            Console.ReadKey();
            Console.Clear();

            SecurityMembraneRunner.RunExample();
        }

        /// <summary>
        /// Runs the load balancing example.
        /// </summary>
        private static void RunLoadBalancingExample()
        {
            Console.Clear();
            Console.WriteLine("⚖️ LOAD BALANCING & DYNAMIC SCALING EXAMPLE");
            Console.WriteLine("============================================");
            Console.WriteLine();
            Console.WriteLine("This example demonstrates:");
            Console.WriteLine("• Dynamic processor membrane creation");
            Console.WriteLine("• Automatic load distribution");
            Console.WriteLine("• System scaling based on load metrics");
            Console.WriteLine("• Failure detection and recovery");
            Console.WriteLine("• Real-time system monitoring");
            Console.WriteLine();
            Console.WriteLine("Press any key to start the example...");
            Console.ReadKey();
            Console.Clear();

            LoadBalancingRunner.RunExample();
        }

        /// <summary>
        /// Runs all examples in sequence.
        /// </summary>
        private static void RunAllExamples()
        {
            Console.Clear();
            Console.WriteLine("🎯 RUNNING ALL EXAMPLES");
            Console.WriteLine("========================");
            Console.WriteLine();
            Console.WriteLine("This will run all three examples in sequence:");
            Console.WriteLine("1. Basic Membrane Operations");
            Console.WriteLine("2. Security Boundaries");
            Console.WriteLine("3. Load Balancing & Dynamic Scaling");
            Console.WriteLine();
            Console.WriteLine("Each example will run automatically with minimal pauses.");
            Console.WriteLine("Press any key to continue or ESC to cancel...");

            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("\nOperation cancelled.");
                return;
            }

            Console.Clear();

            try
            {
                // Run Basic Example
                Console.WriteLine("Starting Basic Membrane Operations Example...");
                BasicMembraneRunner.RunExample();
                
                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("Press any key to continue to Security Example...");
                Console.ReadKey();
                Console.Clear();

                // Run Security Example
                Console.WriteLine("Starting Security Boundaries Example...");
                SecurityMembraneRunner.RunExample();
                
                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("Press any key to continue to Load Balancing Example...");
                Console.ReadKey();
                Console.Clear();

                // Run Load Balancing Example
                Console.WriteLine("Starting Load Balancing Example...");
                LoadBalancingRunner.RunExample();

                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("🎉 All examples completed successfully!");
                Console.WriteLine();
                Console.WriteLine("Key takeaways from the examples:");
                Console.WriteLine("• Membranes provide powerful isolation and control mechanisms");
                Console.WriteLine("• Permeability configuration enables fine-grained access control");
                Console.WriteLine("• Dynamic membrane management supports adaptive systems");
                Console.WriteLine("• Security boundaries can be effectively enforced");
                Console.WriteLine("• Load balancing and scaling can be implemented elegantly");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during example execution: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays help information and documentation links.
        /// </summary>
        private static void DisplayHelpInformation()
        {
            Console.Clear();
            Console.WriteLine("❓ HELP & DOCUMENTATION");
            Console.WriteLine("========================");
            Console.WriteLine();
            Console.WriteLine("📚 Available Documentation:");
            Console.WriteLine();
            Console.WriteLine("1. Complete Membrane Computing Tutorial");
            Console.WriteLine("   • File: docs/MembraneComputingTutorial.md");
            Console.WriteLine("   • Comprehensive guide with advanced patterns");
            Console.WriteLine("   • API reference and troubleshooting");
            Console.WriteLine();
            Console.WriteLine("2. Quick Start Guide");
            Console.WriteLine("   • File: docs/MembraneComputingQuickStart.md");
            Console.WriteLine("   • 5-minute introduction to membrane computing");
            Console.WriteLine("   • Common patterns and examples");
            Console.WriteLine();
            Console.WriteLine("3. Code Examples");
            Console.WriteLine("   • Directory: docs/examples/");
            Console.WriteLine("   • Full source code for all examples");
            Console.WriteLine("   • Ready-to-run implementations");
            Console.WriteLine();
            Console.WriteLine("🌐 Online Resources:");
            Console.WriteLine();
            Console.WriteLine("• HOPE GitHub Repository:");
            Console.WriteLine("  https://github.com/cliftonm/HOPE");
            Console.WriteLine();
            Console.WriteLine("• Membrane Computing Video Tutorial:");
            Console.WriteLine("  http://youtu.be/XoQSTJcrEj8");
            Console.WriteLine();
            Console.WriteLine("• Research Paper on P Systems:");
            Console.WriteLine("  http://en.wikipedia.org/wiki/Membrane_computing");
            Console.WriteLine();
            Console.WriteLine("🛠️ Getting Started:");
            Console.WriteLine();
            Console.WriteLine("1. Read the Quick Start Guide for basic concepts");
            Console.WriteLine("2. Run the Basic Example to see membranes in action");
            Console.WriteLine("3. Explore the Security Example for real-world patterns");
            Console.WriteLine("4. Try the Load Balancing Example for advanced techniques");
            Console.WriteLine("5. Consult the Complete Tutorial for in-depth coverage");
            Console.WriteLine();
            Console.WriteLine("💡 Tips:");
            Console.WriteLine("• Start with simple membrane hierarchies");
            Console.WriteLine("• Always configure permeability explicitly");
            Console.WriteLine("• Monitor membrane performance in production");
            Console.WriteLine("• Use security boundaries for sensitive operations");
            Console.WriteLine("• Consider dynamic scaling for variable workloads");
        }

        /// <summary>
        /// Displays information about membrane computing.
        /// </summary>
        private static void DisplayAboutInformation()
        {
            Console.Clear();
            Console.WriteLine("📖 ABOUT MEMBRANE COMPUTING");
            Console.WriteLine("============================");
            Console.WriteLine();
            Console.WriteLine("🧬 What is Membrane Computing?");
            Console.WriteLine();
            Console.WriteLine("Membrane computing is a computational model inspired by the structure");
            Console.WriteLine("and functioning of living cells. It was introduced by Gheorghe Păun");
            Console.WriteLine("in 1998 as part of the field of natural computing.");
            Console.WriteLine();
            Console.WriteLine("🔬 Biological Inspiration:");
            Console.WriteLine();
            Console.WriteLine("• Cell membranes control molecular transport");
            Console.WriteLine("• Different compartments have specialized functions");
            Console.WriteLine("• Selective permeability maintains cellular integrity");
            Console.WriteLine("• Dynamic reconfiguration enables adaptive behavior");
            Console.WriteLine();
            Console.WriteLine("💻 In HOPE Framework:");
            Console.WriteLine();
            Console.WriteLine("• Membranes contain receptors and other membranes");
            Console.WriteLine("• Permeability controls protocol flow between membranes");
            Console.WriteLine("• Hierarchical organization enables complex architectures");
            Console.WriteLine("• Dynamic reconfiguration supports adaptive systems");
            Console.WriteLine();
            Console.WriteLine("🎯 Key Benefits:");
            Console.WriteLine();
            Console.WriteLine("✅ Isolation         - Separate computational concerns");
            Console.WriteLine("✅ Control          - Fine-grained access management");
            Console.WriteLine("✅ Security         - Enforced boundaries and policies");
            Console.WriteLine("✅ Scalability      - Dynamic system reconfiguration");
            Console.WriteLine("✅ Maintainability  - Clear separation of responsibilities");
            Console.WriteLine();
            Console.WriteLine("🏗️ Common Use Cases:");
            Console.WriteLine();
            Console.WriteLine("• Microservices architecture with controlled communication");
            Console.WriteLine("• Security boundaries for sensitive data processing");
            Console.WriteLine("• Load balancing and dynamic scaling systems");
            Console.WriteLine("• Multi-tenant applications with isolation guarantees");
            Console.WriteLine("• Event-driven architectures with flow control");
            Console.WriteLine();
            Console.WriteLine("🧮 Formal Foundation:");
            Console.WriteLine();
            Console.WriteLine("Membrane computing is based on P systems (Păun systems), which are");
            Console.WriteLine("parallel and distributed computational models. HOPE implements these");
            Console.WriteLine("concepts in a practical software framework.");
            Console.WriteLine();
            Console.WriteLine("📈 Evolution in HOPE:");
            Console.WriteLine();
            Console.WriteLine("HOPE extends traditional membrane computing with:");
            Console.WriteLine("• Semantic type systems for protocol definition");
            Console.WriteLine("• Receptor-based computational units");
            Console.WriteLine("• Dynamic membrane management");
            Console.WriteLine("• Event-driven communication patterns");
            Console.WriteLine("• Runtime reconfiguration capabilities");
            Console.WriteLine();
            Console.WriteLine("🔮 Future Directions:");
            Console.WriteLine();
            Console.WriteLine("• Distributed membrane systems across networks");
            Console.WriteLine("• Machine learning-driven adaptive permeability");
            Console.WriteLine("• Quantum computing integration");
            Console.WriteLine("• Biological process simulation");
        }
    }
}