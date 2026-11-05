import { useRef } from "react";
import { MdAttachFile, MdDeleteOutline, MdDescription } from "react-icons/md";

interface DocumentItem {
  id: string;
  name: string;
  dataType: string;
  fileUrl: string;
}

interface ClinicalDocumentsSectionProps {
  documents: DocumentItem[];
  onChange: (documents: DocumentItem[]) => void;
  disabled?: boolean;
}

export const ClinicalDocumentsSection = ({documents, onChange, disabled = false}: ClinicalDocumentsSectionProps) => {
  const fileInputRef = useRef<HTMLInputElement | null>(null);

  const handleFileUpload = (files: FileList | null) => {
    if (!files || files.length === 0) return;

    const newDocs: DocumentItem[] = Array.from(files).map((file) => ({
      id: crypto.randomUUID(),
      name: file.name,
      dataType: file.type || "application/octet-stream",
      fileUrl: URL.createObjectURL(file),
    }));

    onChange([...documents, ...newDocs]);

    if (fileInputRef.current) {
      fileInputRef.current.value = "";
    }
  };

  const handleRemoveDoc = (id: string) => {
    onChange(documents.filter((doc) => doc.id !== id));
  };

  return (
    <div className="pt-2 border-t border-slate-100">
      <div className="flex items-center justify-between mb-2">
        <label className="text-xs font-semibold text-slate-600 uppercase">
          Clinical Documents
        </label>
        <button
          type="button"
          disabled={disabled}
          onClick={() => fileInputRef.current?.click()}
          className="inline-flex items-center gap-1.5 text-xs font-semibold text-emerald-600 hover:text-emerald-700 bg-emerald-50 hover:bg-emerald-100/70 px-2.5 py-1.5 rounded-md transition-colors cursor-pointer disabled:opacity-50"
        >
          <MdAttachFile className="w-4 h-4" />
          Attach Doc
        </button>
        <input
          ref={fileInputRef}
          type="file"
          multiple
          disabled={disabled}
          className="hidden"
          accept=".pdf,.doc,.docx,.png,.jpg,.jpeg,.xml"
          onChange={(e) => handleFileUpload(e.target.files)}
        />
      </div>

      {documents.length > 0 ? (
        <ul className="space-y-2 mt-2">
          {documents.map((doc) => (
            <li
              key={doc.id}
              className="flex items-center justify-between p-2.5 bg-slate-50 border border-slate-200 rounded-lg text-xs text-slate-700"
            >
              <div className="flex items-center gap-2 truncate">
                <MdDescription className="w-4 h-4 text-emerald-600 shrink-0" />
                <span className="truncate font-medium">{doc.name}</span>
              </div>
              <button
                type="button"
                disabled={disabled}
                onClick={() => handleRemoveDoc(doc.id)}
                className="text-slate-400 hover:text-rose-500 p-1 transition-colors cursor-pointer disabled:opacity-50"
              >
                <MdDeleteOutline className="w-4 h-4" />
              </button>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-xs text-slate-400 italic">No files attached yet.</p>
      )}
    </div>
  );
};