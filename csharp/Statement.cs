//
// Statement.cs: Redland Statement (triple) class. The main means of 
//		 manipulating statement is by the subject, predicate
//		 and object properties.
//
// Author:
//	Cesar Lopez Nataren (cesar@ciencias.unam.mx)
//
// (C) 2004, Cesar Lopez Nataren
//

using System;
using System.Runtime.InteropServices;

namespace Rdf {

	public class Statement : IWrapper {
		
		IntPtr stm;

		public IntPtr Handle {
			get { return stm; }
		}

		public Statement ()
			: this (Redland.World)
		{
		}

		public Statement (Node subject, Node predicate, Node obj)
			: this (Redland.World, subject, predicate, obj)
		{
		}

		[DllImport ("librdf")]
		static extern IntPtr librdf_new_statement (IntPtr world);
		
		private Statement (World world)
		{
			stm = librdf_new_statement (world.Handle);
		}

		[DllImport ("librdf")]
		static extern IntPtr librdf_new_statement_from_nodes (IntPtr world, IntPtr subject, IntPtr predicate, IntPtr obj);
		private Statement (World world, Node subject, Node predicate, Node obj)
		{
			IntPtr subj, pred, o;
			subj = pred = o = IntPtr.Zero;

 			// Console.WriteLine ("Making Statement from {0} {1} {2}", subject.ToString(), predicate.ToString(), obj.ToString());
			
			if((Object)subject != null)
				subj=new Node(subject).Handle;
			if((Object)predicate != null)
				pred=new Node(predicate).Handle;
			if((Object)obj != null)
				o=new Node(obj).Handle;
			stm = librdf_new_statement_from_nodes (world.Handle, subj, pred, o);
 			// Console.WriteLine ("New Statement is {0}", stm.ToString());
		}

		[DllImport ("librdf")]
		static extern void librdf_statement_set_subject (IntPtr stm, IntPtr node);

		public Node Subject {
			set { librdf_statement_set_subject (Handle, value.Handle); }
		}

		[DllImport ("librdf")]
		static extern void librdf_statement_set_predicate (IntPtr stm, IntPtr node);

		public Node Predicate {
			set { librdf_statement_set_predicate (Handle, value.Handle); }
		}

		[DllImport ("librdf")]
		static extern void librdf_statement_set_object (IntPtr statement, IntPtr node);

		public Node Object {
			set { librdf_statement_set_object (stm, value.Handle); }
		}

		internal Statement (IntPtr raw)
		{
			stm = raw;
		}

		[DllImport ("librdf")]
		static extern IntPtr librdf_statement_to_string (IntPtr stm);

		public override string ToString ()
		{
			IntPtr istr=librdf_statement_to_string (stm);
                        return Marshal.PtrToStringAuto(istr);
		}
	}
}