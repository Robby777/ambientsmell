import java.math.BigInteger;


public class JavaSmells {
	
	/**
	 * A message chain is a statement that contains a sequence of method 
	 * invocations or instance variable accesses.
	 */
	public void MessageChains() {
		// Chained methods containing the same object
		String mm = "FOO".toLowerCase().substring(1).replace('o', 'm');
		
		// Chained methods containing different objects
		boolean no = new BigInteger("1234567890").
			divideAndRemainder(new BigInteger("123"))[1].toString().equals(mm);
		
		// Chained instance variables
		String foo = new BadClass().brc.bc.bc.s;
	}
	
	class BadClass {
		public BadClass bc = new BadClass();
		public BadderClass brc = new BadderClass();
		public String 	s = "foo";

	}
	
	class BadderClass {
		public BadClass bc = new BadClass();	
	}
}
