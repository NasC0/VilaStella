      *** IMPORTANT NOTE: 
	  Changes in Web.config and in the configuration file:
      Since version 1.9.2 the section name in Web.config has changed from name="fileUpload" to name="backload" (see below). 
      The root element must also be changed in your config file from <fileUpload> to <backload>
      The ConfigurationSection class has changed to <section type="Backload.Configuration.BackloadSection ..." />
	  Backload has implemented a fallback routine for the old schema, but it is best practice to update your config files.
	  See examples on GitHub.
	  
	  Release notes:
	  Version 1.9.3.0:
	  - NET 4.0 support added.
	  - Improved read/write access to storage locations at runtime.
	  - APTCA added to the assembly and compliance to the Security-Transparent Code (Level 2) rules.
	  - Added maxLength to the Images configuration. Images above this limit (bytes) will be stored directly with a generic thumbnail and are not handled in the image processing sub pipeline. 
	  - Added forceObjectContext attribute [true|false] (default: false) to the security configuration. If true, a client must send an objectContext with a request.
	  
	  Bug fixes:
	  - Issue #21 resolved, where an exception occured while trying to read readonly files.
	  - Issue #20 resolved, where in some environments uploading very large files (> 200MB) could cause an out of memory exception.
	  - Issue #17, #22, #23 resolved, where initialization of the component fails in some environments.
	  - Issue with urls resolved, where backload generates a wrong url if the application runs in a virtual or sub directory of web root.
