using System.Collections.Generic;

namespace security_scan.Models
{
    public class MockScanResult
    {
        public List<MockS3Bucket> S3Buckets { get; set; }
        public List<MockIamPolicy> IamPolicies { get; set; }
        public List<MockSecurityGroup> SecurityGroups { get; set; }
    }

    public class MockS3Bucket
    {
        public string Name { get; set; }
        public bool PublicAccess { get; set; }
    }

    public class MockIamPolicy
    {
        public string Name { get; set; }
        public bool IsTooPermissive { get; set; }
    }

    public class MockSecurityGroup
    {
        public string Name { get; set; }
        public List<int> OpenPorts { get; set; }
    }
}
