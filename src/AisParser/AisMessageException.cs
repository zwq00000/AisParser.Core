using System;

namespace AisParser {
    public class AisMessageException : Exception {
        public AisMessageException() {
        }

        public AisMessageException(string str) : base(str) {
        }
    }
}