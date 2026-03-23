import React, { useState, useEffect } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";

import ParentDashboard from "./pages/ParentDashboard";
import { addDependant } from "./api/dependantApi";

interface School {
    id: number;
    name: string;
}

const CreateDependant: React.FC = () => {
    const [fullName, setFullName] = useState("");
    const [admissionNumber, setAdmissionNumber] = useState("");
    const [classLevel, setClassLevel] = useState("");
    const [schoolId, setSchoolId] = useState(0);
    const [schools, setSchools] = useState<School[]>([]);
    const [loading, setLoading] = useState(false);

    // fetch schools from backend
    useEffect(() => {
        const fetchSchools = async () => {
            try {
                const res = await fetch("/api/schools");
                if (!res.ok) throw new Error("Failed to fetch schools");
                const data: School[] = await res.json();
                setSchools(data);
                if (data.length > 0) setSchoolId(data[0].id);
            } catch (err) {
                console.error(err);
            }
        };
        fetchSchools();
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            // No need for parentId: backend assigns it
            await addDependant({ fullName, admissionNumber, classLevel, schoolId });
            alert("Dependant added successfully!");
            window.location.href = "/parent"; // redirect to dashboard
        } catch (err) {
            console.error(err);
            alert("Failed to add dependant.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Add Dependant</h2>
            <form onSubmit={handleSubmit}>
                <div className="mb-3">
                    <input
                        className="form-control"
                        placeholder="Full Name"
                        value={fullName}
                        onChange={(e) => setFullName(e.target.value)}
                        required
                    />
                </div>
                <div className="mb-3">
                    <input
                        className="form-control"
                        placeholder="Admission Number"
                        value={admissionNumber}
                        onChange={(e) => setAdmissionNumber(e.target.value)}
                        required
                    />
                </div>
                <div className="mb-3">
                    <input
                        className="form-control"
                        placeholder="Class Level"
                        value={classLevel}
                        onChange={(e) => setClassLevel(e.target.value)}
                        required
                    />
                </div>
                <div className="mb-3">
                    <select
                        className="form-control"
                        value={schoolId}
                        onChange={(e) => setSchoolId(Number(e.target.value))}
                        required
                    >
                        {schools.map((s) => (
                            <option key={s.id} value={s.id}>
                                {s.name}
                            </option>
                        ))}
                    </select>
                </div>
                <button className="btn btn-primary" type="submit" disabled={loading}>
                    {loading ? "Saving..." : "Add Dependant"}
                </button>
            </form>
        </div>
    );
};

const CreateFeeRequest: React.FC = () => {
    window.location.href = "/FeeRequest/Create";
    return null;
};

const App: React.FC = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/parent" element={<ParentDashboard />} />
                <Route path="/parent/create-dependant" element={<CreateDependant />} />
                <Route path="/parent/create-fee-request" element={<CreateFeeRequest />} />
            </Routes>
        </BrowserRouter>
    );
};

export default App;