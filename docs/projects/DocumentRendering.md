# BusinessAppFramework.DocumentRendering

**Layer:** infrastructure. **Depends on:** Application, Domain. **Used by:** the host application.

## Role

DocumentRendering turns a domain object into a printable document. A module provides an HTML template and a loader that gathers the data; the framework renders the template and produces HTML or PDF.

## What's inside

- `HtmlDocumentTemplate` and `HtmlDocumentTemplateRegistry`: a template is a layout resource plus a content resource, both embedded in an assembly. Templates are registered under a document key.
- `IDocumentModelLoader`, `DocumentModelLoaderFactory`, `DocumentModelLoaderTypeRegistry`: a loader fetches the data a document needs from a domain object id. The registry associates a loader with a document key.
- `HtmlDocumentRenderer`: renders the template with Scriban, then drives a headless Chromium through PuppeteerSharp to produce a PDF. A single browser instance is shared and shut down with the process.
- `PdfPageLayout`: page size, margins and orientation.

## In practice

A module registers a template and a model loader for, say, an invoice. The `DocumentGeneratorController` in Adapter then serves an HTML preview and a PDF download for any invoice id, and the UI exposes them through document generation application actions.
