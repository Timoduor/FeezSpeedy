// parent-dashboard/src/api/dependantApi.ts
import { Dependant } from "../types";

export const fetchDependants = async (): Promise<Dependant[]> => {
    const res = await fetch("/api/dependants");
    if (!res.ok) throw new Error("Failed to fetch dependants");
    return res.json();
};

export const addDependant = async (dep: Dependant): Promise<Dependant> => {
    const res = await fetch("/api/dependants", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dep),
    });
    if (!res.ok) throw new Error("Failed to add dependant");
    return res.json();
};