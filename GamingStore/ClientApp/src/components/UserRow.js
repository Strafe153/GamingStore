import React, { Component} from 'react';

export class UserRow extends Component {
    static displayName = UserRow.name;

    constructor(props) {
        super(props);

        this.state = {
            token: sessionStorage.getItem('token')
        };
    }

    async deleteUser(id) {
        await fetch(`../api/users/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            }
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .catch(error => alert(error.message));
    }

    updateUser(id) {
        window.location.href = `/users/edit/${id}`;
    }

    render() {
        return <tr>
            <td>{ this.props.id }</td>
            <td>{ this.props.username }</td>
            <td>{ this.props.role }</td>
            <td>
            {(this.props.calledFromAdmin || (sessionStorage.getItem('username') === this.props.username)) &&
                <div className="text-center d-flex justify-content-around">
                    <button onClick={() => this.updateUser(this.props.id)} className="btn btn-sm btn-info text-white">Edit</button>
                    <button onClick={
                        async () => { 
                            await this.deleteUser(this.props.id); 
                            await this.props.getUsers(); 
                        }
                    } className="btn btn-sm btn-danger">Delete</button>
                </div>
            }
            </td>
        </tr>;
    }
}