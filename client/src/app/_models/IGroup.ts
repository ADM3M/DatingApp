export interface IGroup {
    name: string;
    connections: Connection[];
}

interface Connection {
    connectionId: string;
    userName: string;
}