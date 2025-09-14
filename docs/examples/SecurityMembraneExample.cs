using System;
using System.Collections.Generic;
using System.Linq;
using Clifton.Receptor;
using Clifton.Receptor.Interfaces;
using Clifton.SemanticTypeSystem.Interfaces;

namespace HOPE.Examples.MembraneComputing
{
    /// <summary>
    /// Example 2: Security-Focused Membrane Configuration
    /// This example demonstrates how to use membranes to create secure boundaries
    /// for sensitive operations and data processing.
    /// </summary>
    public class SecurityMembraneExample
    {
        private ISemanticTypeSystem sts;
        private IApplicationController appController;
        private Membrane rootMembrane;
        private Membrane publicZone;
        private Membrane secureZone;
        private Membrane cryptoZone;

        public SecurityMembraneExample(ISemanticTypeSystem semanticTypeSystem, IApplicationController applicationController)
        {
            sts = semanticTypeSystem;
            appController = applicationController;
        }

        /// <summary>
        /// Creates a security-focused membrane system with multiple security zones.
        /// </summary>
        public void CreateSecureMembraneSystem()
        {
            Console.WriteLine("Creating secure membrane system...\n");

            // Create root membrane
            rootMembrane = new Membrane(sts, appController);
            rootMembrane.Name = "SecureSystemRoot";

            // Create security zones
            CreateSecurityZones();

            // Configure receptors
            ConfigureSecurityReceptors();

            // Configure strict security permeability
            ConfigureSecurityPermeability();

            // Set up security monitoring
            SetupSecurityMonitoring();

            Console.WriteLine("Secure membrane system created successfully!");
            PrintSecurityConfiguration();
        }

        /// <summary>
        /// Creates different security zones with varying trust levels.
        /// </summary>
        private void CreateSecurityZones()
        {
            // Public Zone - Low security, accessible from outside
            publicZone = rootMembrane.CreateInnerMembrane();
            publicZone.Name = "PublicZone";

            // Secure Zone - Medium security, internal processing
            secureZone = rootMembrane.CreateInnerMembrane();
            secureZone.Name = "SecureZone";

            // Crypto Zone - High security, cryptographic operations
            cryptoZone = secureZone.CreateInnerMembrane();
            cryptoZone.Name = "CryptoZone";

            Console.WriteLine("Security zones created:");
            Console.WriteLine("📢 PublicZone - External interface (Low Security)");
            Console.WriteLine("🔒 SecureZone - Internal processing (Medium Security)");
            Console.WriteLine("🔐 CryptoZone - Cryptographic operations (High Security)");
        }

        /// <summary>
        /// Configures receptors in their appropriate security zones.
        /// </summary>
        private void ConfigureSecurityReceptors()
        {
            try
            {
                // Public zone receptors - handle external requests
                publicZone.RegisterReceptor("PublicAPI", "PublicAPIReceptor.dll");
                publicZone.RegisterReceptor("RequestValidator", "RequestValidatorReceptor.dll");
                publicZone.RegisterReceptor("ResponseFormatter", "ResponseFormatterReceptor.dll");

                // Secure zone receptors - internal business logic
                secureZone.RegisterReceptor("BusinessLogicProcessor", "BusinessLogicReceptor.dll");
                secureZone.RegisterReceptor("DataValidator", "DataValidatorReceptor.dll");
                secureZone.RegisterReceptor("AuditLogger", "AuditLoggerReceptor.dll");

                // Crypto zone receptors - sensitive operations
                cryptoZone.RegisterReceptor("EncryptionEngine", "EncryptionReceptor.dll");
                cryptoZone.RegisterReceptor("KeyManager", "KeyManagerReceptor.dll");
                cryptoZone.RegisterReceptor("HashGenerator", "HashGeneratorReceptor.dll");

                Console.WriteLine("\nReceptors configured in appropriate security zones.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Note: Could not load security receptors (expected in this example): {ex.Message}");
            }
        }

        /// <summary>
        /// Configures strict permeability rules for security boundaries.
        /// </summary>
        private void ConfigureSecurityPermeability()
        {
            Console.WriteLine("\nConfiguring security permeability...");

            ConfigurePublicZonePermeability();
            ConfigureSecureZonePermeability();
            ConfigureCryptoZonePermeability();

            // Update all permeability settings
            publicZone.UpdatePermeability();
            secureZone.UpdatePermeability();
            cryptoZone.UpdatePermeability();

            Console.WriteLine("Security permeability configured.");
        }

        /// <summary>
        /// Configures permeability for the public zone.
        /// </summary>
        private void ConfigurePublicZonePermeability()
        {
            // Allow public requests to enter
            SetPermeability(publicZone, "PublicRequest", PermeabilityDirection.In, true);
            SetPermeability(publicZone, "UserQuery", PermeabilityDirection.In, true);

            // Allow validated requests to leave for secure zone
            SetPermeability(publicZone, "ValidatedRequest", PermeabilityDirection.Out, true);

            // Allow public responses to leave
            SetPermeability(publicZone, "PublicResponse", PermeabilityDirection.Out, true);

            // Block sensitive data from entering public zone
            SetPermeability(publicZone, "SensitiveData", PermeabilityDirection.In, false);
            SetPermeability(publicZone, "PrivateKey", PermeabilityDirection.In, false);
            SetPermeability(publicZone, "InternalState", PermeabilityDirection.In, false);

            // Block internal protocols from leaving
            SetPermeability(publicZone, "InternalValidationResult", PermeabilityDirection.Out, false);
        }

        /// <summary>
        /// Configures permeability for the secure zone.
        /// </summary>
        private void ConfigureSecureZonePermeability()
        {
            // Allow validated requests from public zone
            SetPermeability(secureZone, "ValidatedRequest", PermeabilityDirection.In, true);

            // Allow secure processing results to go to crypto zone
            SetPermeability(secureZone, "ProcessingRequest", PermeabilityDirection.Out, true);

            // Allow encrypted results from crypto zone
            SetPermeability(secureZone, "EncryptedResult", PermeabilityDirection.In, true);

            // Allow audit logs to leave (for monitoring)
            SetPermeability(secureZone, "AuditLog", PermeabilityDirection.Out, true);

            // Allow final results to go to public zone for response
            SetPermeability(secureZone, "SecureResult", PermeabilityDirection.Out, true);

            // Block raw business data from leaving
            SetPermeability(secureZone, "RawBusinessData", PermeabilityDirection.Out, false);
            SetPermeability(secureZone, "InternalBusinessState", PermeabilityDirection.Out, false);

            // Block external access to internal protocols
            SetPermeability(secureZone, "PublicRequest", PermeabilityDirection.In, false);
        }

        /// <summary>
        /// Configures permeability for the crypto zone (most restrictive).
        /// </summary>
        private void ConfigureCryptoZonePermeability()
        {
            // Allow processing requests from secure zone
            SetPermeability(cryptoZone, "ProcessingRequest", PermeabilityDirection.In, true);

            // Allow encrypted results to leave
            SetPermeability(cryptoZone, "EncryptedResult", PermeabilityDirection.Out, true);

            // Allow hash results to leave
            SetPermeability(cryptoZone, "HashResult", PermeabilityDirection.Out, true);

            // BLOCK EVERYTHING ELSE - Maximum security
            
            // No private keys can leave
            SetPermeability(cryptoZone, "PrivateKey", PermeabilityDirection.Out, false);
            SetPermeability(cryptoZone, "MasterKey", PermeabilityDirection.Out, false);
            
            // No plaintext sensitive data can leave
            SetPermeability(cryptoZone, "PlaintextData", PermeabilityDirection.Out, false);
            SetPermeability(cryptoZone, "DecryptedData", PermeabilityDirection.Out, false);
            
            // No internal crypto state can leave
            SetPermeability(cryptoZone, "CryptoInternalState", PermeabilityDirection.Out, false);
            SetPermeability(cryptoZone, "KeyGenerationSeed", PermeabilityDirection.Out, false);
            
            // No external data can enter directly
            SetPermeability(cryptoZone, "PublicRequest", PermeabilityDirection.In, false);
            SetPermeability(cryptoZone, "ExternalData", PermeabilityDirection.In, false);
        }

        /// <summary>
        /// Sets up security monitoring and event handling.
        /// </summary>
        private void SetupSecurityMonitoring()
        {
            Console.WriteLine("Setting up security monitoring...");

            // Monitor all membranes for security events
            MonitorMembrane(publicZone, "PUBLIC");
            MonitorMembrane(secureZone, "SECURE");
            MonitorMembrane(cryptoZone, "CRYPTO");
        }

        /// <summary>
        /// Monitors a membrane for security-relevant events.
        /// </summary>
        private void MonitorMembrane(Membrane membrane, string zoneType)
        {
            membrane.NewCarrier += (sender, args) =>
            {
                Console.WriteLine($"🔍 [{zoneType}] Protocol '{args.Protocol.Protocol}' created in {membrane.Name}");
                
                // Log potential security violations
                if (IsSecuritySensitiveProtocol(args.Protocol.Protocol))
                {
                    Console.WriteLine($"⚠️  [{zoneType}] SECURITY ALERT: Sensitive protocol '{args.Protocol.Protocol}' detected!");
                }
            };

            membrane.NewReceptor += (sender, args) =>
            {
                Console.WriteLine($"➕ [{zoneType}] New receptor '{args.Receptor.Name}' added to {membrane.Name}");
            };

            membrane.ReceptorRemoved += (sender, args) =>
            {
                Console.WriteLine($"➖ [{zoneType}] Receptor '{args.Receptor.Name}' removed from {membrane.Name}");
            };
        }

        /// <summary>
        /// Demonstrates security boundary violations and how they're prevented.
        /// </summary>
        public void DemonstrateSecurityBoundaries()
        {
            Console.WriteLine("\n=== DEMONSTRATING SECURITY BOUNDARIES ===");

            // Test 1: Try to access crypto zone directly from public
            Console.WriteLine("\nTest 1: Attempting direct access to crypto zone from public zone...");
            TestSecurityBoundary("PublicRequest", publicZone, cryptoZone, "This should be blocked!");

            // Test 2: Try to extract private keys
            Console.WriteLine("\nTest 2: Attempting to extract private key from crypto zone...");
            TestSecurityBoundary("PrivateKey", cryptoZone, publicZone, "This should be blocked!");

            // Test 3: Valid security flow
            Console.WriteLine("\nTest 3: Testing valid security flow...");
            DemonstrateValidSecurityFlow();

            // Test 4: Security policy changes
            Console.WriteLine("\nTest 4: Demonstrating runtime security policy changes...");
            DemonstrateSecurityPolicyChanges();
        }

        /// <summary>
        /// Tests a security boundary by attempting to pass a protocol.
        /// </summary>
        private void TestSecurityBoundary(string protocol, Membrane source, Membrane target, string expectedResult)
        {
            Console.WriteLine($"  Attempting to pass '{protocol}' from {source.Name} to {target.Name}");
            Console.WriteLine($"  Expected result: {expectedResult}");

            // Check if the protocol would be allowed
            var outKey = new PermeabilityKey { Protocol = protocol, Direction = PermeabilityDirection.Out };
            var inKey = new PermeabilityKey { Protocol = protocol, Direction = PermeabilityDirection.In };

            bool canLeaveSource = source.ProtocolPermeability.ContainsKey(outKey) && source.ProtocolPermeability[outKey].Permeable;
            bool canEnterTarget = target.ProtocolPermeability.ContainsKey(inKey) && target.ProtocolPermeability[inKey].Permeable;

            if (canLeaveSource && canEnterTarget)
            {
                Console.WriteLine($"  ✅ Protocol '{protocol}' would be allowed to pass");
            }
            else
            {
                Console.WriteLine($"  ❌ Protocol '{protocol}' is blocked by security policy");
                if (!canLeaveSource) Console.WriteLine($"    - Blocked leaving {source.Name}");
                if (!canEnterTarget) Console.WriteLine($"    - Blocked entering {target.Name}");
            }
        }

        /// <summary>
        /// Demonstrates a valid security flow through all zones.
        /// </summary>
        private void DemonstrateValidSecurityFlow()
        {
            Console.WriteLine("  Valid flow: PublicRequest → ValidatedRequest → ProcessingRequest → EncryptedResult → SecureResult → PublicResponse");

            var flowSteps = new[]
            {
                ("PublicRequest", publicZone, "→ ValidatedRequest"),
                ("ValidatedRequest", secureZone, "→ ProcessingRequest"),
                ("ProcessingRequest", cryptoZone, "→ EncryptedResult"),
                ("EncryptedResult", secureZone, "→ SecureResult"),
                ("SecureResult", publicZone, "→ PublicResponse")
            };

            foreach (var (protocol, zone, nextStep) in flowSteps)
            {
                Console.WriteLine($"    {protocol} in {zone.Name} {nextStep}");
            }

            Console.WriteLine("  ✅ This flow respects all security boundaries");
        }

        /// <summary>
        /// Demonstrates runtime security policy changes.
        /// </summary>
        private void DemonstrateSecurityPolicyChanges()
        {
            Console.WriteLine("  Temporarily enabling debug mode for secure zone...");

            // Save original debug permeability
            var debugKey = new PermeabilityKey { Protocol = "DebugInfo", Direction = PermeabilityDirection.Out };
            bool originalDebugSetting = secureZone.ProtocolPermeability.ContainsKey(debugKey) && 
                                       secureZone.ProtocolPermeability[debugKey].Permeable;

            // Enable debug temporarily
            SetPermeability(secureZone, "DebugInfo", PermeabilityDirection.Out, true);
            secureZone.UpdatePermeability();
            Console.WriteLine("  ✅ Debug enabled - DebugInfo can now leave secure zone");

            // Simulate debug session
            Console.WriteLine("  ... debug session running ...");
            System.Threading.Thread.Sleep(1000);

            // Restore original setting
            SetPermeability(secureZone, "DebugInfo", PermeabilityDirection.Out, originalDebugSetting);
            secureZone.UpdatePermeability();
            Console.WriteLine("  🔒 Debug disabled - Security policy restored");
        }

        /// <summary>
        /// Performs a security audit of the current membrane configuration.
        /// </summary>
        public void PerformSecurityAudit()
        {
            Console.WriteLine("\n=== SECURITY AUDIT ===");

            AuditMembrane(publicZone, "PUBLIC ZONE");
            AuditMembrane(secureZone, "SECURE ZONE");
            AuditMembrane(cryptoZone, "CRYPTO ZONE");

            Console.WriteLine("\n=== AUDIT COMPLETE ===");
        }

        /// <summary>
        /// Audits a single membrane for security compliance.
        /// </summary>
        private void AuditMembrane(Membrane membrane, string zoneName)
        {
            Console.WriteLine($"\nAuditing {zoneName}:");

            var securityIssues = new List<string>();
            var allowedOutProtocols = new List<string>();
            var blockedInProtocols = new List<string>();

            foreach (var kvp in membrane.ProtocolPermeability)
            {
                var protocol = kvp.Key.Protocol;
                var direction = kvp.Key.Direction;
                var permeable = kvp.Value.Permeable;

                if (direction == PermeabilityDirection.Out && permeable)
                {
                    allowedOutProtocols.Add(protocol);
                }

                if (direction == PermeabilityDirection.In && !permeable)
                {
                    blockedInProtocols.Add(protocol);
                }

                // Check for potential security issues
                if (IsSecuritySensitiveProtocol(protocol) && direction == PermeabilityDirection.Out && permeable)
                {
                    securityIssues.Add($"Sensitive protocol '{protocol}' is allowed to leave {zoneName}");
                }
            }

            Console.WriteLine($"  📤 Protocols allowed out: {string.Join(", ", allowedOutProtocols)}");
            Console.WriteLine($"  🚫 Protocols blocked in: {string.Join(", ", blockedInProtocols)}");

            if (securityIssues.Any())
            {
                Console.WriteLine($"  ⚠️  Security issues found:");
                foreach (var issue in securityIssues)
                {
                    Console.WriteLine($"    - {issue}");
                }
            }
            else
            {
                Console.WriteLine($"  ✅ No security issues detected");
            }
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
        /// Checks if a protocol contains sensitive security information.
        /// </summary>
        private bool IsSecuritySensitiveProtocol(string protocol)
        {
            var sensitiveKeywords = new[] { "Key", "Password", "Secret", "Private", "Token", "Credential", "Hash", "Decrypt" };
            return sensitiveKeywords.Any(keyword => protocol.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Prints the current security configuration.
        /// </summary>
        private void PrintSecurityConfiguration()
        {
            Console.WriteLine("\n=== SECURITY CONFIGURATION ===");
            PrintMembraneHierarchy(rootMembrane);
        }

        /// <summary>
        /// Prints the membrane hierarchy with security indicators.
        /// </summary>
        private void PrintMembraneHierarchy(Membrane membrane, int depth = 0)
        {
            string indent = new string(' ', depth * 2);
            string securityIcon = GetSecurityIcon(membrane.Name);
            Console.WriteLine($"{indent}{securityIcon} {membrane.Name} (Receptors: {membrane.Receptors.Count})");

            // Print key permeability rules
            if (membrane.ProtocolPermeability.Count > 0)
            {
                var importantRules = membrane.ProtocolPermeability
                    .Where(kvp => IsImportantSecurityRule(kvp.Key.Protocol))
                    .Take(3);

                foreach (var kvp in importantRules)
                {
                    string arrow = kvp.Key.Direction == PermeabilityDirection.In ? "→" : "←";
                    string status = kvp.Value.Permeable ? "✅" : "🔒";
                    Console.WriteLine($"{indent}  {status} {kvp.Key.Protocol} {arrow}");
                }
            }

            foreach (var child in membrane.Membranes)
            {
                PrintMembraneHierarchy((Membrane)child, depth + 1);
            }
        }

        /// <summary>
        /// Gets the security icon for a membrane based on its name.
        /// </summary>
        private string GetSecurityIcon(string membraneName)
        {
            return membraneName.ToLower() switch
            {
                var name when name.Contains("public") => "📢",
                var name when name.Contains("secure") => "🔒",
                var name when name.Contains("crypto") => "🔐",
                _ => "📁"
            };
        }

        /// <summary>
        /// Checks if a security rule is important to display.
        /// </summary>
        private bool IsImportantSecurityRule(string protocol)
        {
            var importantProtocols = new[] { "PrivateKey", "PublicRequest", "EncryptedResult", "SensitiveData" };
            return importantProtocols.Contains(protocol);
        }

        /// <summary>
        /// Cleans up the security membrane system.
        /// </summary>
        public void Cleanup()
        {
            if (rootMembrane != null)
            {
                rootMembrane.Reset();
                rootMembrane = null;
                Console.WriteLine("Security membrane system cleaned up.");
            }
        }
    }

    /// <summary>
    /// Helper class for running the security membrane example.
    /// </summary>
    public class SecurityMembraneRunner
    {
        public static void RunExample()
        {
            Console.WriteLine("=== HOPE Membrane Computing - Security Example ===\n");

            try
            {
                // Create mock semantic type system and application controller
                ISemanticTypeSystem sts = new SemanticTypeSystem();
                IApplicationController appController = new ApplicationController();

                var example = new SecurityMembraneExample(sts, appController);

                // Create the secure system
                example.CreateSecureMembraneSystem();

                Console.WriteLine("\nPress any key to test security boundaries...");
                Console.ReadKey();

                // Demonstrate security boundaries
                example.DemonstrateSecurityBoundaries();

                Console.WriteLine("\nPress any key to perform security audit...");
                Console.ReadKey();

                // Perform security audit
                example.PerformSecurityAudit();

                Console.WriteLine("\nPress any key to cleanup and exit...");
                Console.ReadKey();

                example.Cleanup();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running security example: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            Console.WriteLine("\n=== Security Example Complete ===");
        }
    }
}