import React from "react";
import { School } from "../types";

interface Props { schools: School[]; }

const SchoolsTable: React.FC<Props> = ({ schools }) => (
    <table className="table table-bordered mt-4">
        <thead className="table-dark">
            <tr>
                <th>#</th>
                <th>School Name</th>
                <th>Location</th>
            </tr>
        </thead>
        <tbody>
            {schools.length === 0 ? (
                <tr>
                    <td colSpan={3} className="text-center">No schools yet.</td>
                </tr>
            ) : (
                schools.map((s, idx) => (
                    <tr key={s.id}>
                        <td>{idx + 1}</td>
                        <td>{s.name}</td>
                        <td>{s.location || "-"}</td>
                    </tr>
                ))
            )}
        </tbody>
    </table>
);

export default SchoolsTable;